using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tdd.Models
{
    [Serializable]
    public class TowerType
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public int Damage { get; set; }

        public int Speed { get; set; }

        public Cost Cost { get; set; }
    }
}