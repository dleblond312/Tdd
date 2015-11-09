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

namespace Tdd.Controllers
{
    public class GameHub : Hub
    {
        private readonly IGameService gameService;
        public GameHub(IGameService gameService)
        {
            this.gameService = gameService;
        }
        //// Call the broadcastMessage method to update clients.
        //Clients.All.broadcastMessage(name, message);

        public async Task Send(string name, string message)
        {
            switch(name)
            {
                case "start":
                    var gameRoom = await this.gameService.StartGameAsync(this.Context);
                    Clients.Client(this.Context.ConnectionId).receiveCommand("gameCreated", JsonConvert.SerializeObject(gameRoom));
                    break;
                default:
                    Clients.Client(this.Context.ConnectionId).receiveCommand("unknownCommand", name);
                    break;
            }
        }
    }
}