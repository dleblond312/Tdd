﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            foreach (Tower tower in room.Towers)
            {
                if (tower.ReadyAt <= DateTime.UtcNow)
                {
                    foreach (Mob mob in round.Mobs)
                    {
                        if (Point.IsNear(tower.Location, mob.CurrentLocation, tower.Range))
                        {
                            round.Projectiles.Add(new Projectile(tower, mob));
                            tower.ReadyAt = DateTime.UtcNow.AddMilliseconds(tower.Speed); // TODO: Should have a Game constant scale modifier here
                            break;
                        }
                    }
                }
            }

            foreach(Projectile projectile in round.Projectiles.Reverse())
            {
                var span = DateTime.UtcNow.Subtract(projectile.LastUpdated);
                projectile.Location = Point.TrackTo(projectile.Location, projectile.Target.CurrentLocation, (span.Milliseconds * (projectile.Speed / Constants.GameSpeed)));
                if (Point.IsNear(projectile.Location, projectile.Target.CurrentLocation, 0.1))
                {
                    projectile.Target.Health -= projectile.Damage;
                    round.Projectiles.Remove(projectile);
                    if (projectile.Target.Health <= 0)
                    {
                        round.Mobs.Remove(projectile.Target);
                    }
                }

                projectile.LastUpdated = DateTime.UtcNow;
            }

        }

    }
}