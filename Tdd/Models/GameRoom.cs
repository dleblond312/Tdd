using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Tdd.Models
{
    [Serializable]
    public class GameRoom
    {
        public int Id { get; set; }

        public int MapSize { get; set; }

        public IList<Player> Players { get; set; }

        public int CurrentRound { get; set; }

        public DateTime CurrentRoundStartTime { get; set; }

        public IDictionary<Point, Tower> Towers { get; set; }

        public IList<Point> Map { get; set; }

        public GameRoom(HubCallerContext context)
        {
            this.Id = (new Random()).Next(1000); ;
            this.Players = new List<Player>()
            {
                new Player(context, 0)
            };
            this.CurrentRound = 0;
            this.Towers = new Dictionary<Point, Tower>();
            this.Map = Constants.Map;
            this.MapSize = Constants.MapSize;
        }
    }
}