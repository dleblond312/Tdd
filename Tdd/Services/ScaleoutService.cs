using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Tdd.Controllers;
using Tdd.Models;

namespace Tdd.Services
{
    public class ScaleoutService : IScaleoutService
    {
        private readonly IDictionary<string, List<HubCallerContext>> subscriptions;

        public ScaleoutService()
        {
            this.subscriptions = new Dictionary<string, List<HubCallerContext>>();
        }

        public void Subscribe(Persist type, object id, HubCallerContext connection)
        {
            if(!this.subscriptions.ContainsKey(this.GetKey(type, id)))
            {
                this.subscriptions.Add(this.GetKey(type, id), new List<HubCallerContext>());
            }
            this.subscriptions[this.GetKey(type, id)].Add(connection);
        }

        public void Store(Persist type, object id, object o)
        {
            HttpRuntime.Cache.Add(this.GetKey(type, id), o, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(60), System.Web.Caching.CacheItemPriority.Normal, null);
            this.Notify(type, id, o);
        }

        public async Task<object> Get(Persist type, object id)
        {
            return HttpRuntime.Cache.Get(this.GetKey(type, id));
        }

        public async Task<object> Remove(Persist type, object id)
        {
            return HttpRuntime.Cache.Remove(this.GetKey(type, id));
        }

        private string GetKey(Persist type, object name)
        {
            return type.ToString() + "-" + name.ToString();
        }

        public void Notify(Persist type, object id, object o)
        {
            if (subscriptions.ContainsKey(this.GetKey(type, id)))
            {
                var context = GlobalHost.ConnectionManager.GetHubContext<GameHub>();
                foreach (var clientContext in subscriptions[this.GetKey(type, id)])
                {
                    context.Clients.Client(clientContext.ConnectionId).propertyUpdated(this.GetKey(type, id), o);
                }
            }
        }
    }
}