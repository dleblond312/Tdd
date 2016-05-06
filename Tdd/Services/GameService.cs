using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
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
            // gameRoom.Towers = gameRoom.Towers.Union(TestingConstants.Maze).ToList(); // TODO remove test code
            this.scaleoutService.Subscribe(Persist.GameRoom, gameRoom.Id, context);
            this.scaleoutService.Store(Persist.GameRoom, gameRoom.Id, gameRoom);
            return gameRoom;
        }

        /// <summary>
        /// Confirms that the user is part of the roomId provided.
        /// Throws 401 if the roomId is not found
        /// Throws 500 if the user is not part of the roomId
        /// </summary>
        /// <param name="context">The context to confirm is present in the room</param>
        /// <param name="roomId">The roomId to check against</param>
        /// <returns>True only if the context can be found in the gameRoom</returns>
        public async Task<bool> IsValidGameRoomUser(HubCallerContext context, string roomId)
        {
            var gameRoom = this.scaleoutService.Get(Persist.GameRoom, roomId) as GameRoom;

            if(gameRoom != null && !string.IsNullOrWhiteSpace(roomId))
            {
                foreach(Player player in gameRoom.Players)
                {
                    if(player.Context.ConnectionId == context.ConnectionId)
                    {
                        return true;
                    }
                }
                throw new HttpException(500, "User is not authorized for game room ");
            }
            throw new HttpException(401, "Game room not found");
        }

        public async Task<GameRoom> JoinGameAsync(HubCallerContext context, string roomId)
        {
            var gameRoom = this.scaleoutService.Get(Persist.GameRoom, roomId) as GameRoom;

            if(gameRoom != null)
            {
                gameRoom.Players.Add(new Player(context, gameRoom.Players.Count));
                this.scaleoutService.Subscribe(Persist.GameRoom, gameRoom.Id, context);
                this.scaleoutService.Store(Persist.GameRoom, gameRoom.Id, gameRoom);
            }

            return gameRoom;
        }

        public async Task<GameRoom> IncrementRoundAsync(HubCallerContext context, string roomId)
        {

            var gameRoom = this.scaleoutService.Get(Persist.GameRoom, roomId) as GameRoom;

            foreach(var player in gameRoom.Players)
            {
                this.scaleoutService.Subscribe(Persist.GameRound, roomId, player.Context);
            }

            var success = await this.gameRoundService.ProcessRoundAsync(roomId);
            if(success)
            {
                lock(gameRoom)
                {
                    gameRoom.CurrentRound += 1;
                    gameRoom.CurrentRoundStartTime = DateTime.UtcNow;
                    this.scaleoutService.Notify(Persist.GameRoom, gameRoom.Id, gameRoom);
                    return gameRoom;
                }
            }

            return null;
        }

        public async Task BuildTower(HubCallerContext context, string roomId, string towerId, int x, int y)
        {
            var gameRoom = this.scaleoutService.Get(Persist.GameRoom, roomId) as GameRoom;
            
            lock(gameRoom)
            {
                if (!string.IsNullOrWhiteSpace(towerId))
                {
                    var location = new Point(x, y);
                    if (gameRoom.Towers.ContainsKey(location))
                    {
                        throw new HttpException(400, "Existing tower conflicts with build location");
                    }

                    int parsed;
                    if(!int.TryParse(towerId, out parsed))
                    {
                        throw new HttpException(400, "Tower Id non-integer value");
                    }

                    var currentPlayer = gameRoom.Players.Where(p => p.Context.ConnectionId == context.ConnectionId).First();
                    var towerToBuild = Constants.TowerTypes.Where(t => (int)t.Id == parsed).First();
                    if (currentPlayer.Resources.CanAfford(towerToBuild.Cost))
                    {
                        if (currentPlayer.Resources.Subtract(towerToBuild.Cost))
                        {
                            gameRoom.Towers.Add(location, new Tower(towerToBuild, context.ConnectionId, location, towerId));
                            var round = this.scaleoutService.Get(Persist.GameRound, roomId) as GameRound;
                            if(round != null)
                            {
                                foreach(var mob in round.Mobs)
                                {
                                    mob.Path = null; // The path is no longer valid for the next update
                                }
                                this.scaleoutService.Store(Persist.GameRound, roomId, round);
                            }
                        }
                    }

                    this.scaleoutService.Notify(Persist.GameRoom, gameRoom.Id, gameRoom);
                }
            }
        }
    }
}