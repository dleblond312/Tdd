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

                    var abilities = (IDictionary<string, object>)projectile.Target.Type.Abilities;

                    // Evasion game mechanic
                    if (abilities.ContainsKey("evasion"))
                    {
                        var roll = random.Value.Next(100);
                        if(roll < (double)projectile.Target.Type.Abilities.evasion)
                        {
                            projectile.Target.Health -= projectile.Damage;
                        }
                    }
                    else
                    {
                        projectile.Target.Health -= projectile.Damage;
                    }
                    round.Projectiles.Remove(projectile);
                    if (projectile.Target.Health <= 0)
                    {
                        // Fracture game mechanic
                        if(abilities.ContainsKey("fracture"))
                        {
                            var count = (int)projectile.Target.Type.Abilities.fracture.count;
                            for (var i = 0; i < count; i++)
                            {
                                round.Mobs.Add(new Mob()
                                {
                                    Type = (MobType) projectile.Target.Type.Abilities.fracture.shard,
                                    CurrentLocation = new Point(projectile.Target.CurrentLocation),
                                    EndingLocation = new Point(projectile.Target.EndingLocation),
                                    Health = ((MobType) projectile.Target.Type.Abilities.fracture.shard).StartingHealth

                                });
                            }
                        }

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