using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Tdd.Models
{
    [Serializable]
    public class Projectile
    {
        public Projectile(Tower tower, Mob mob)
        {
            this.Location = new Point(tower.Location);
            this.LastUpdated = DateTime.UtcNow;
            this.Owner = tower.Owner;
            this.Target = mob;
            this.Speed = tower.ProjectileSpeed;
            this.Damage = tower.Damage;
            this.TowerType = tower.Type;
        }

        public Point Location { get; set; }

        [JsonIgnore]
        public DateTime LastUpdated { get; set; }

        [JsonIgnore]
        public Player Owner { get; private set; }

        [JsonIgnore]
        public Mob Target { get; private set; }

        [JsonIgnore]
        public double Speed { get; private set; }

        [JsonIgnore]
        public int Damage { get; private set; }

        [JsonIgnore]
        public Constants.TowerList TowerType { get; set; }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} -> {1}", this.Location, this.Target.CurrentLocation);
        }

    }
}