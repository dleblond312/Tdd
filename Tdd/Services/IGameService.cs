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
        Task<bool> IsValidGameRoomUser(HubCallerContext context, string roomId);
        Task BuildTower(HubCallerContext context, string roomId, string towerId, int x, int y);
    }
}
