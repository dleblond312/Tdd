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
        public string Owner { get; set; }

        public Point Location { get; set; }

        public string Id { get; set; }

        public int Speed { get; set; }

        public int Damage { get; set; }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}: {1}", this.Owner, this.Location.ToString());
        }
    }
}