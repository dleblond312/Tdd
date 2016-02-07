using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tdd.Models;

namespace Tdd.Services
{
    public class TowerProjectileService : ITowerProjectileService
    {
        private readonly IScaleoutService scaleoutService;

        public TowerProjectileService(IScaleoutService scaleoutService)
        {
            this.scaleoutService = scaleoutService;
        }

        public void UpdateProjectiles(GameRoom room, GameRound round)
        {
            foreach(Tower tower in room.Towers)
            {
                if(tower.ReadyAt <= DateTime.UtcNow.AddMilliseconds(tower.Speed))
                {
                    foreach(Mob mob in round.Mobs)
                    {
                        if(Point.isNear(tower.Location, mob.CurrentLocation, tower.Range)) {
                            round.Projectiles.Add(new Projectile(tower, mob));
                            tower.ReadyAt = DateTime.UtcNow.AddMilliseconds(tower.ProjectileSpeed);
                            break;
                        }
                    }
                }

                foreach (Projectile projectile in round.Projectiles.Reverse())
                {
                    if (Point.isNear(projectile.Location, projectile.Target.CurrentLocation, 0.1))
                    {
                        projectile.Target.Health -= projectile.Damage;
                        round.Projectiles.Remove(projectile);
                    }
                    else
                    {
                        var span = DateTime.UtcNow.Subtract(projectile.LastUpdated);
                        var vectorX = projectile.Target.CurrentLocation.X - projectile.Location.X;
                        var vectorY = projectile.Target.CurrentLocation.Y - projectile.Location.Y;

                        projectile.Location.X = projectile.Location.X + (span.TotalMilliseconds * vectorX / Constants.GameSpeed);
                        projectile.Location.Y = projectile.Location.Y + (span.TotalMilliseconds * vectorY / Constants.GameSpeed);
                    }
                }
            }
        }


        public void RemoveDeadMobs(GameRoom room, GameRound round)
        {
            round.Mobs.ToList().RemoveAll(m => m.Health <= 0);
        }
    }
}