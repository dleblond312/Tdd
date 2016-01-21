using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tdd.Models
{
    [Serializable]
    public class Resources
    {
        public int Primary { get; set; }

        public int Income { get; set; }

        public bool Subtract(Resources other)
        {
            if(this.Primary >= other.Primary)
            {
                this.Primary = this.Primary - other.Primary;
                return true;
            }
            return false;
        }

        public bool CanAfford(Resources cost)
        {
            return this.Primary >= cost.Primary;
        }
    }
}