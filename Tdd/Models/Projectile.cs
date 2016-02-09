using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tdd.Models
{
    [Serializable]
    public class Projectile
    {
        public Projectile(Tower tower, Mob mob)
        {
            this.Location = tower.Location;
            this.LastUpdated = DateTime.UtcNow;
            this.Owner = tower.Owner;
            this.Target = mob;
            this.Speed = tower.ProjectileSpeed;
            this.Damage = tower.Damage;
        }

        public Point Location { get; set; }

        public DateTime LastUpdated { get; set; }

        public string Owner { get; private set; }

        public Mob Target { get; private set; }

        public int Speed { get; private set; }

        public int Damage { get; private set; }


    }
}