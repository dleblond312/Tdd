using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using Tdd.Models;

namespace Tdd.Services
{
    public interface IChatService
    {
        Task ChatMessageReceived(HubCallerContext context, string roomId, string message);
    }
}
