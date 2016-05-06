using Newtonsoft.Json;
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
            this.LastUpdated = DateTime.UtcNow; // Default last updated to now
        }

        [JsonIgnore] // Pretty costly to send over the network
        public MobType Type { get; set; }

        public int Health { get; set; }

        public Point CurrentLocation { get; set; }

        [JsonIgnore]
        public Point EndingLocation { get; set; }

        [JsonIgnore]
        public DateTime LastUpdated { get; set; }

        [JsonIgnore]
        public double CurrentSpeed { get; set; }

        public dynamic Status { get; private set; }

        [JsonIgnore]
        public IEnumerable<GamePoint> Path { get; set; }

    }
}