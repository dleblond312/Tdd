using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Tdd.Models
{
    [Serializable]
    public class Tower
    {
        public Tower(TowerType towerType, Player owner, Point location)
        {
            this.Type = towerType.Id;
            this.Speed = towerType.Speed;
            this.ProjectileSpeed = towerType.ProjectileSpeed;
            this.Range = towerType.Range;
            this.Owner = owner;
            this.Damage = towerType.Damage;
            this.Location = location;
            this.Effects = towerType.Effects;
        }

        public Constants.TowerList Type { get; private set; }

        public Player Owner { get; private set; }

        public Point Location { get; private set; }

        public int Speed { get; private set; }

        public int Damage { get; private set; }

        public double Range { get; private set; }

        public int ProjectileSpeed { get; private set; }

        public dynamic Effects { get; private set; }

        [JsonIgnore]
        public DateTime ReadyAt { get; set; }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}: {1}", this.Owner, this.Location.ToString());
        }
    }
}