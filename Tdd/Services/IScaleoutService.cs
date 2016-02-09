using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tdd.Models;

namespace Tdd.Services
{
    public interface IScaleoutService
    {
        void Subscribe(Persist type, object id, HubCallerContext connection);

        void Store(Persist type, object id, object o);

        object Get(Persist type, object id);

        Task<object> Remove(Persist type, object id);
        void Notify(Persist type, object id, object o);
    }
}
