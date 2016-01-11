using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;
using Tdd.Models;
using Newtonsoft.Json;
using Tdd.Controllers;
using Microsoft.AspNet.SignalR;

namespace Tdd.Services
{
    public class ChatService : IChatService
    {
        private readonly IScaleoutService scaleoutService;
        private readonly IGameService gameService;

        public ChatService(IScaleoutService scaleoutService, IGameService gameService)
        {
            this.scaleoutService = scaleoutService;
            this.gameService = gameService;
        }

        public async Task ChatMessageReceived(HubCallerContext context, string roomId, string message)
        {
            var gameRoom = await this.scaleoutService.Get(Persist.GameRoom, roomId) as GameRoom;
            var players = gameRoom.Players;

            var gameHubContext = GlobalHost.ConnectionManager.GetHubContext<GameHub>();
            foreach (Player player in players) 
            {
                gameHubContext.Clients.Client(player.Context.ConnectionId).chatMessageReceived(message);
            }
        }


    }
}