using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tdd.Models
{
    public class MobType
    {

        public int StartingHealth { get; set; }
        
        public int MoveSpeed { get; set; }

        public double Armor { get; set; }

        public double Evasion { get; set; }
    }
}