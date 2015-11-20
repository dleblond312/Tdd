using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using Tdd.Models;

namespace Tdd.Services
{
    public class GameService : IGameService
    {
        private readonly IScaleoutService scaleoutService;
        private readonly IGameRoundService gameRoundService;

        public GameService(IScaleoutService scaleoutService, IGameRoundService gameRoundService) {
            this.scaleoutService = scaleoutService;
            this.gameRoundService = gameRoundService;
        }

        public async Task<GameRoom> StartGameAsync(HubCallerContext context)
        {
            var gameRoom = new GameRoom(context);
            this.scaleoutService.Subscribe(Persist.GameRoom, gameRoom.Id, context);
            this.scaleoutService.Store(Persist.GameRoom, gameRoom.Id, gameRoom);

            return gameRoom;
        }

        public async Task<GameRoom> JoinGameAsync(HubCallerContext context, string roomId)
        {
            var gameRoom = await this.scaleoutService.Get(Persist.GameRoom, roomId) as GameRoom;

            if(gameRoom != null)
            {
                gameRoom.Players.Add(new Player(context));
                this.scaleoutService.Subscribe(Persist.GameRoom, gameRoom.Id, context);
                this.scaleoutService.Store(Persist.GameRoom, gameRoom.Id, gameRoom);
            }

            return gameRoom;
        }

        public async Task<GameRoom> IncrementRoundAsync(HubCallerContext context, string roomId)
        {

            var gameRoom = await this.scaleoutService.Get(Persist.GameRoom, roomId) as GameRoom;
            foreach(var player in gameRoom.Players)
            {
                this.scaleoutService.Subscribe(Persist.GameRound, roomId, player.Context);
            }

            var success = await this.gameRoundService.ProcessRoundAsync(roomId);
            if(success)
            {
                gameRoom.CurrentRound += 1;
                gameRoom.CurrentRoundStartTime = DateTime.UtcNow;
                this.scaleoutService.Store(Persist.GameRoom, gameRoom.Id, gameRoom);
                return gameRoom;
            }

            return null;
        }
    }
}