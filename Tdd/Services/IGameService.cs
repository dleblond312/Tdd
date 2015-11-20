using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tdd.Models;

namespace Tdd.Services
{
    public interface IGameService
    {
        Task<GameRoom> StartGameAsync(HubCallerContext context);
        Task<GameRoom> IncrementRoundAsync(HubCallerContext context, string roomId);
        Task<GameRoom> JoinGameAsync(HubCallerContext context, string roomId);
    }
}
