using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using Tdd.Models;
using Tdd.Utils;

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
            foreach (var tower in room.Towers.Values)
            {
                if (tower.Damage > 0 && tower.ReadyAt <= DateTime.UtcNow)
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
                if (projectile.Location == projectile.Target.CurrentLocation) // Inefficient to call Point.IsNear(projectile.Location, projectile.Target.CurrentLocation, 0.1)
                {
                    TowerType towerType;
                    switch(projectile.TowerType)
                    {
                        case Constants.TowerList.Slowing:
                            towerType = GameDataUtils.GetTowerTypeFromTowerList(projectile.TowerType);
                            projectile.Target.Status.slow = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(towerType.Effects.slow));
                            projectile.Target.Status.slow.remaining = DateTime.UtcNow.AddMilliseconds((int)towerType.Effects.slow.duration);
                            break;
                        case Constants.TowerList.Dot:
                            towerType = GameDataUtils.GetTowerTypeFromTowerList(projectile.TowerType);
                            projectile.Target.Status.dot = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(towerType.Effects.dot));
                            if(!((projectile.Target.Status.dot as IDictionary<string, object>)?.ContainsKey("lastUpdated") ?? false))
                            {
                                projectile.Target.Status.dot.lastUpdated = DateTime.UtcNow;
                            }
                            projectile.Target.Status.dot.remaining = DateTime.UtcNow.AddMilliseconds((int)towerType.Effects.dot.duration);
                            break;
                        default:
                            // No-op
                            break;
                    }
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

        public void TickDots(GameRoom room, GameRound round)
        {
            foreach(Mob mob in round.Mobs.Reverse())
            {
                if((mob.Status as IDictionary<string, object>)?.ContainsKey("dot") ?? false)
                {
                    var span = DateTime.UtcNow.Subtract((DateTime)mob.Status.dot.lastUpdated);

                    if(span.TotalMilliseconds > 500)
                    {
                        mob.Health -= (int)mob.Status.dot.damage;
                        mob.Status.dot.lastUpdated = DateTime.UtcNow;

                        if(DateTime.UtcNow > (DateTime)mob.Status.dot.remaining)
                        {
                            (mob.Status as IDictionary<string, object>).Remove("dot");
                        }

                        if(mob.Health <= 0)
                        {
                            round.Mobs.Remove(mob);
                        }
                    }
                }
            }
        }
    }
}