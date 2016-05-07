using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tdd.Models
{
    [Serializable]
    public class GameRound
    {
        public IList<Mob> Mobs {get; set;}

        public IList<Projectile> Projectiles { get; set; }

        public int RemainingMobs { get; set; }
    }
}