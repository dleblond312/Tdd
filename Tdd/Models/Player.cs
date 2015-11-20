using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Tdd.Models
{
    [Serializable]
    public class Player
    {
        public Player(HubCallerContext context)
        {
            this.Context = context;
            this.StartingLocation = Constants.StartingLocations[0];
            this.EndingLocation = Constants.EndingLocations[0];
            this.CurrentLife = Constants.StartingLife;
        }

        [JsonIgnore]
        public HubCallerContext Context {get; set;}

        public int CurrentLife { get; set; }

        public Point StartingLocation { get; set; }

        public Point EndingLocation { get; set; }
    }
}