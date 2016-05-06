using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace Tdd.Models
{
    public class MobType
    {
        public MobType()
        {
            this.Abilities = new ExpandoObject(); // Default this to empty
        }

        public string Name { get; set; }

        public int StartingHealth { get; set; }
        
        public int MoveSpeed { get; set; }

        /// <summary>
        /// Abilities list:
        /// Evasion: double -- Dodges % of attacks
        /// Fracture: { count: int, shard : MobType } -- Splits into {count} of units of type {shard}
        /// TODO Avenger: { range: double, bonus : double }  -- On death, gives all nearby enemies +{bonus}% move speed 
        /// TODO Recovery: { step: int, value: int } -- Every {step}ms gain {value} health
        /// TODO Flicker: { cooldown : int, duration: int} -- On take damage, goes invicible for {duration}ms. Cannot occur more then once every {cooldown}ms
        /// TODO Stoneskin: int -- Reduce every attack by n
        /// </summary>
        public dynamic Abilities { get; set; } 

    }
}