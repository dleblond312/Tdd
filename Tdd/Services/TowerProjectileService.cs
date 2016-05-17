using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using Tdd.Models;
using Tdd.Utils;

namespace Tdd.Services
{
    public class TowerProjectileService : ITowerProjectileService
    {
        private readonly IScaleoutService scaleoutService;

        // Thread safe randomness http://stackoverflow.com/questions/19270507/correct-way-to-use-random-in-multithread-application
        static int seed = Environment.TickCount;
        static readonly ThreadLocal<Random> random = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));

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
                        if (Point.IsNear(tower.Location, mob.Location, tower.Range))
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
                projectile.Location = Point.TrackTo(projectile.Location, projectile.Target.Location, (span.Milliseconds * (projectile.Speed / Constants.GameSpeed)));
                if (Point.IsNear(projectile.Location, projectile.Target.Location, 0.1))
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

                    var abilities = projectile.Target.Type.Abilities;
                    var damage = projectile.Damage;

                    // Stoneskin game mechanic
                    if (abilities?.Stoneskin.HasValue == true)
                    {
                        damage -= abilities.Stoneskin.Value;
                    }

                    // Evasion game mechanic
                    if (abilities?.Evasion.HasValue == true)
                    {
                        var roll = random.Value.Next(100);
                        if(roll < abilities.Evasion.Value)
                        {
                            damage = 0;
                        }
                    }

                    projectile.Target.Health -= Math.Max(damage, 0);
                    round.Projectiles.Remove(projectile);

                    if (projectile.Target.Health <= 0)
                    {
                        // Rewards game mechanic
                        if(abilities?.Rewards != null)
                        {
                            var roll = random.Value.Next(100);
                            if(roll < abilities.Rewards.Percent)
                            {
                                projectile.Owner.Resources.Income += Math.Max(abilities.Rewards.Income, 0);
                                projectile.Owner.Resources.Primary += Math.Max(abilities.Rewards.Primary, 0);
                                projectile.Owner.Resources.Research += Math.Max(abilities.Rewards.Research, 0);
                            }
                        }

                        // Fracture game mechanic
                        if(abilities?.Fracture != null)
                        {
                            for (var i = 0; i < abilities.Fracture.Count; i++)
                            {
                                round.Mobs.Add(new Mob(
                                    projectile.Target.Type.Abilities.Fracture.Shard, 
                                    new Point(projectile.Target.Location), 
                                    new Point(projectile.Target.EndingLocation))
                                );
                            }
                        }

                        // Avenger game mechanic
                        if(abilities?.Avenger != null)
                        {
                            var range = projectile.Target.Type.Abilities.Avenger.Range;
                            foreach(var mob in round.Mobs.Reverse())
                            {
                                if(Point.IsNear(projectile.Target.Location, mob.Location, range))
                                {
                                    mob.CurrentSpeed *= (1 + projectile.Target.Type.Abilities.Avenger.Bonus);
                                }
                            }
                        }

                        round.Mobs.Remove(projectile.Target);
                    }
                }

                projectile.LastUpdated = DateTime.UtcNow;
            }

        }

        public void TickDots(Mob mob, GameRoom room, GameRound round)
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