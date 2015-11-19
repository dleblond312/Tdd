using System;
using System.Collections.Generic;
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

        public GameRoundService(IScaleoutService scaleoutService)
        {
            this.scaleoutService = scaleoutService;
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

                new Thread(() =>
                {
                    var totalMobs = currentRound.RemainingMobs; // snapshot of total count
                   for(int i = 0; i < totalMobs; i++)
                    {
                        currentRound.Mobs.Add(new Mob()
                        {
                            Health = Constants.MobTypes[0].StartingHealth,
                            Type = Constants.MobTypes[0]
                        });
                        currentRound.RemainingMobs--;
                        this.scaleoutService.Store(Persist.GameRound, roomId, currentRound);

                        Thread.Sleep(Constants.RoundPauseMs);
                    }

                    this.scaleoutService.Remove(Persist.GameRound, roomId);
                }).Start();

                return true;
            }
            return false;
        }
    }
}