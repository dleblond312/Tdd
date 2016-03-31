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
        private readonly IMobMovementService mobMovementService;
        private readonly ITowerProjectileService towerProjectileService;

        public GameRoundService(IScaleoutService scaleoutService, IPathingService pathingService, IMobMovementService mobMovementService, ITowerProjectileService towerProjectileService)
        {
            this.scaleoutService = scaleoutService;
            this.pathingService = pathingService;
            this.mobMovementService = mobMovementService;
            this.towerProjectileService = towerProjectileService;
        }

        public async Task<bool> ProcessRoundAsync(string roomId)
        {
            var currentRound = this.scaleoutService.Get(Persist.GameRound, roomId) as GameRound;

            if(currentRound == null)
            {
                currentRound = new GameRound()
                {
                    Mobs = new List<Mob>(),
                    Projectiles = new List<Projectile>(),
                    RemainingMobs = Constants.RoundSize
                };

                this.scaleoutService.Store(Persist.GameRound, roomId, currentRound);

                // Move
                new Thread(async () =>
                {
                    GameRoom room = this.scaleoutService.Get(Persist.GameRoom, roomId) as GameRoom;
                    var startTime = DateTime.UtcNow;
                    var endTime = startTime.AddMinutes(60);

                    var round = this.scaleoutService.Get(Persist.GameRound, roomId) as GameRound;
                    if (round != null)
                    {

                        while (round.Mobs.Count > 0 || round.RemainingMobs > 0)
                        {
                            if (DateTime.UtcNow > endTime)
                            {
                                return; // Prevents runaway threads
                            }
                            
                            lock (room)
                            {
                                round = this.scaleoutService.Get(Persist.GameRound, roomId) as GameRound;
                                foreach (var mob in round.Mobs.Reverse())
                                {
                                    this.mobMovementService.RemoveMobsAtEnding(mob, room, round);
                                    this.mobMovementService.UpdateMobLocation(mob, room, round);
                                    this.towerProjectileService.UpdateProjectiles(room, round);
                                    this.scaleoutService.Store(Persist.GameRound, roomId, round);
                                }
                            }
                            Thread.Sleep(30); // Makes ~30 FPS
                        }
                        
                        lock (room)
                        {
                            round.Projectiles.Clear();
                            this.scaleoutService.Store(Persist.GameRound, roomId, round);
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
                new Thread(() =>
                {
                   var totalMobs = currentRound.RemainingMobs; // snapshot of total count

                   for(int i = 0; i < totalMobs; i++)
                    {
                        GameRoom room = this.scaleoutService.Get(Persist.GameRoom, roomId) as GameRoom;
                        lock(room)
                        {
                            var round = this.scaleoutService.Get(Persist.GameRound, roomId) as GameRound;
                            for(var j = 0; j < room.Players.Count; j++)
                            {
                            round.Mobs.Add(new Mob()
                            {
                                Health = Constants.MobTypes[0].StartingHealth,
                                Type = Constants.MobTypes[0],
                                    CurrentLocation = room.Players[j].StartingLocation,
                                    EndingLocation = room.Players[j].EndingLocation,
                                LastUpdated = DateTime.UtcNow

                            });
                            }
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