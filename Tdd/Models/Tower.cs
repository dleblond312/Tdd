using System;
using System.Collections.Generic;
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
    }
}