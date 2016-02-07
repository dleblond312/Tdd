using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tdd.Models;

namespace Tdd.Services
{
    public class MobMovementService : IMobMovementService
    {
        private readonly IScaleoutService scaleoutService;
        private readonly IPathingService pathingService;

        public MobMovementService(IScaleoutService scaleoutService, IPathingService pathingService)
        {
            this.scaleoutService = scaleoutService;
            this.pathingService = pathingService;
        }

        public void RemoveMobsAtEnding(Mob mob, GameRoom room, GameRound round)
        {
            if (Point.isNear(mob.CurrentLocation, mob.EndingLocation, 0.5))
            {
                foreach (var player in room.Players)
                {
                    if (mob.EndingLocation.X == player.EndingLocation.X && mob.EndingLocation.Y == player.EndingLocation.Y)
                    {
                        player.CurrentLife -= 1;
                        round.Mobs.Remove(mob);
                        this.scaleoutService.Store(Persist.GameRoom, room.Id, room);
                        break;
                    }
                }
            }
        }

        public void UpdateMobLocation(Mob mob, GameRoom room, GameRound round)
        {
            double x;
            double y;
            var span = DateTime.UtcNow.Subtract(mob.LastUpdated);

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

                if (next != null)
                {

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
                else
                {
                    mob.CurrentLocation = new Point(mob.EndingLocation.X, mob.EndingLocation.Y);
                }

                mob.LastUpdated = DateTime.UtcNow;
            }
        }
    }
}