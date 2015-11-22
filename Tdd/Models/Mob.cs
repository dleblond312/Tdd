using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tdd.Models
{
    [Serializable]
    public class Mob
    {
        public MobType Type { get; set; }

        public int Health { get; set; }

        public Point CurrentLocation { get; set; }

        public Point EndingLocation { get; set; }

        public DateTime LastUpdated { get; set; }

    }
}