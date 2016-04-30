using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace Tdd.Models
{
    [Serializable]
    public class Mob
    {
        public Mob()
        {
            this.Status = new ExpandoObject(); // Default the status to an empty dictionary
        }

        public MobType Type { get; set; }

        public int Health { get; set; }

        public Point CurrentLocation { get; set; }

        public Point EndingLocation { get; set; }

        public DateTime LastUpdated { get; set; }

        //public List<GamePoint> Path { get; set; } // Debugging

        public dynamic Status { get; private set; }

    }
}