using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Tdd.Models;

namespace Tdd.Services
{
    public class GameRoundService : IGameRoundService
    {
        private readonly IScaleoutService scaleoutService;
        private readonly IPathingService pathingService;

        public GameRoundService(IScaleoutService scaleoutService, IPathingService pathingService)
        {
            this.scaleoutService = scaleoutService;
            this.pathingService = pathingService;
        }

        public async Task<bool> ProcessRoundAsync(string roomId)
        {
            var currentRound = await this.scaleoutService.Get(Persist.GameRound, roomId) as GameRound;

            if(currentRound == null)
            {
                currentRound = new GameRound()
                {
                    Mobs = new List<Mob>(),
                    RemainingMobs = Constants.RoundSize
                };

                this.scaleoutService.Store(Persist.GameRound, roomId, currentRound);

                object syncObj = new Object();

                // Move
                new Thread(async () =>
                {
                    var startTime = DateTime.UtcNow;
                    var endTime = startTime.AddMinutes(60);

                    var round = await this.scaleoutService.Get(Persist.GameRound, roomId) as GameRound;
                    if (round != null)
                    {

                        while (round.Mobs.Count > 0 || round.RemainingMobs > 0)
                        {
                            if (DateTime.UtcNow > endTime)
                            {
                                return;
                            }
                            
                            round = await this.scaleoutService.Get(Persist.GameRound, roomId) as GameRound;
                            GameRoom room = await this.scaleoutService.Get(Persist.GameRoom, roomId) as GameRoom;
                            lock (syncObj)
                            {
                                foreach (var mob in round.Mobs.Reverse())
                                {
                                    double x;
                                    double y;
                                    var span = DateTime.UtcNow.Subtract(mob.LastUpdated);

                                    if(Point.isNear(mob.CurrentLocation, mob.EndingLocation, 0.5))
                                    {
                                        foreach(var player in room.Players)
                                        {
                                            if(mob.EndingLocation.X == player.EndingLocation.X && mob.EndingLocation.Y == player.EndingLocation.Y)
                                            {
                                                player.CurrentLife -= 1;
                                                round.Mobs.Remove(mob);
                                                this.scaleoutService.Store(Persist.GameRoom, roomId, room);
                                                break;
                                            }
                                        }
                                    }

                                    if (span.Milliseconds > 0)
                                    {
                                        var path = this.pathingService.FindPath<GamePoint>(new GamePoint(room, mob.CurrentLocation), new GamePoint(room, mob.EndingLocation), (p1, p2) =>
                                        {
                                            // Euclidian Squared heuristic
                                            var dx = p1.X - p2.X;
                                            var dy = p1.Y - p2.Y;
                                            return dx * dx + dy * dy;
                                        }, (p) =>
                                        {
                                            // Euclidian squared heuristic estimate
                                            var dx = p.X - mob.EndingLocation.X;
                                            var dy = p.Y - mob.EndingLocation.Y;
                                            return dx * dx + dy * dy;
                                        });

                                        var next = path.Reverse().Skip(1).FirstOrDefault();

                                        if(next != null)
                                        {

                                            //var vector = new Point(next.X - mob.CurrentLocation.X, next.Y - mob.CurrentLocation.Y);
                                            //mob.CurrentLocation = new Point(
                                            //    mob.CurrentLocation.X + (vector.X * (span.TotalMilliseconds / Constants.GameSpeed)),
                                            //    mob.CurrentLocation.Y + (vector.Y * (span.TotalMilliseconds / Constants.GameSpeed))
                                            //);

                                            if (mob.CurrentLocation.X == next.X)
                                            {
                                                x = mob.CurrentLocation.X;
                                            }
                                            else if (mob.CurrentLocation.X < next.X)
                                            {
                                                x = Math.Min(mob.CurrentLocation.X + (span.TotalMilliseconds / Constants.GameSpeed), next.X);
                                            }
                                            else
                                            {
                                                x = Math.Max(mob.CurrentLocation.X - (span.TotalMilliseconds / Constants.GameSpeed), next.X);
                                            }

                                            if(mob.CurrentLocation.Y >= 11 || next.Y >= 11)
                                            {
                                                Console.WriteLine("Too high");
                                            }

                                            if (mob.CurrentLocation.Y == next.Y)
                                            {
                                                y = mob.CurrentLocation.Y;
                                            }
                                            else if (mob.CurrentLocation.Y < next.Y)
                                            {
                                                y = Math.Min(mob.CurrentLocation.Y + (span.TotalMilliseconds / Constants.GameSpeed), next.Y);
                                            }
                                            else
                                            {
                                                y = Math.Max(mob.CurrentLocation.Y - (span.TotalMilliseconds / Constants.GameSpeed), next.Y);
                                            }

                                            mob.CurrentLocation = new Point(x, y);
                                        }

                                        mob.LastUpdated = DateTime.UtcNow;
                                    }
                                }
                                this.scaleoutService.Store(Persist.GameRound, roomId, round);
                            }
                            Thread.Sleep(25);
                        }

                        await this.scaleoutService.Remove(Persist.GameRound, roomId);
                    }
                    else
                    {
                        Debug.WriteLine("Mob spawning thread ended early.");
                        return;
                    }
                }).Start();

                // Spawn mobs
                new Thread(async () =>
                {
                   var totalMobs = currentRound.RemainingMobs; // snapshot of total count

                   for(int i = 0; i < totalMobs; i++)
                    {
                        var round = await this.scaleoutService.Get(Persist.GameRound, roomId) as GameRound;
                        lock(syncObj)
                        {
                            round.Mobs.Add(new Mob()
                            {
                                Health = Constants.MobTypes[0].StartingHealth,
                                Type = Constants.MobTypes[0],
                                CurrentLocation = Constants.StartingLocations[0],
                                EndingLocation = Constants.EndingLocations[0],
                                LastUpdated = DateTime.UtcNow

                            });
                            round.RemainingMobs--;
                        }

                        Thread.Sleep(Constants.RoundPauseMs);
                    }
                }).Start();

                return true;
            }
            return false;
        }
    }
}