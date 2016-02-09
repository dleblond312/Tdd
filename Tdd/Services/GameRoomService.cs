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
            var currentRound = await this.scaleoutService.Get(Persist.GameRound, roomId) as GameRound;

            if(currentRound == null)
            {
                currentRound = new GameRound()
                {
                    Mobs = new List<Mob>(),
                    Projectiles = new List<Projectile>(),
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
                                return; // Prevents runaway threads
                            }
                            
                            round = await this.scaleoutService.Get(Persist.GameRound, roomId) as GameRound;
                            GameRoom room = await this.scaleoutService.Get(Persist.GameRoom, roomId) as GameRoom;
                            lock (syncObj)
                            {
                                foreach (var mob in round.Mobs.Reverse())
                                {
                                    this.mobMovementService.RemoveMobsAtEnding(mob, room, round);
                                    this.mobMovementService.UpdateMobLocation(mob, room, round);
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

                // Fire projectiles
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
                                return; // Prevents runaway threads
                            }
                            round = await this.scaleoutService.Get(Persist.GameRound, roomId) as GameRound;
                            GameRoom room = await this.scaleoutService.Get(Persist.GameRoom, roomId) as GameRoom;

                            this.towerProjectileService.UpdateProjectiles(room, round);

                            //lock(syncObj)
                            //{
                            //    this.towerProjectileService.RemoveDeadMobs(room, round);
                            //}
                            Thread.Sleep(25);
                        }
                    }
                }).Start();

                return true;
            }
            return false;
        }
    }
}