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

        public DateTime LastUpdated { get; set; }

        public string Owner { get; private set; }

        public Mob Target { get; private set; }

        public double Speed { get; private set; }

        public int Damage { get; private set; }

        public Constants.TowerList TowerType { get; set; }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} -> {1}", this.Location, this.Target.CurrentLocation);
        }

    }
}