using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Tdd.Models;
using System.Security.Principal;
using Tdd.Services;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Tdd.Helpers;
using System.Diagnostics;

namespace Tdd.Controllers
{
    public class GameHub : Hub
    {
        private readonly IGameService gameService;
        private readonly IChatService chatService;

        public GameHub(IGameService gameService, IChatService chatService)
        {
            this.gameService = gameService;
            this.chatService = chatService;
            
        }
        //// Call the broadcastMessage method to update clients.
        //Clients.All.broadcastMessage(name, message);

        public async Task Send(string name, string message)
        {
            GameRoom gameRoom;

            switch(name)
            {
                case "createGame":
                    gameRoom = await this.gameService.StartGameAsync(this.Context);
                    break;
                case "joinGame":
                    gameRoom = await this.gameService.JoinGameAsync(this.Context, message);
                    break;
                case "startRound":
                    gameRoom = await this.gameService.IncrementRoundAsync(this.Context, message);
                    break;
                default:
                    Clients.Client(this.Context.ConnectionId).warn("unknownCommand", name);
                    break;
            }
        }

        public async Task ChatMessageSend(string roomId, string message)
        {
            if(await this.gameService.IsValidGameRoomUser(this.Context, roomId))
            {
                await this.chatService.ChatMessageReceived(this.Context, roomId, message);
            }
        }

        public async Task BuildTower(string roomId, string towerId, int x, int y)
        {
            if(await this.gameService.IsValidGameRoomUser(this.Context, roomId))
            {
                await this.gameService.BuildTower(this.Context, roomId, towerId, x, y);
            }
        }
    }
}