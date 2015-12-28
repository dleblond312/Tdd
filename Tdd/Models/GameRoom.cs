﻿using Microsoft.AspNet.SignalR.Hubs;
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

        public IList<Player> Players { get; set; }

        public int CurrentRound { get; set; }

        public DateTime CurrentRoundStartTime { get; set; }

        public GameRoom(HubCallerContext context)
        {
            this.Id = (new Random()).Next(1000); ;
            this.Players = new List<Player>()
            {
                new Player(context)
            };
            this.CurrentRound = 0;
        }
    }
}