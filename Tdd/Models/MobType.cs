using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace Tdd.Models
{
    public class MobType
    {
        public string Name { get; set; }

        public int StartingHealth { get; set; }
        
        public int MoveSpeed { get; set; }

        /// <summary>
        /// TODO Recovery: { step: int, value: int } -- Every {step}ms gain {value} health
        /// TODO Flicker: { cooldown : int, duration: int} -- On take damage, goes invicible for {duration}ms. Cannot occur more then once every {cooldown}ms
        /// TODO Necromancer, spawn dudes over time
        /// </summary>
        public Abilities Abilities { get; set; } 

    }

    public class Abilities
    {
        /// <summary>
        /// Dodges {evasion}% of attacks
        /// </summary>
        public double? Evasion { get; set; }

        /// <summary>
        /// Splits into {count} of units of type {shard}
        /// </summary>
        public FractureAbility Fracture { get; set; }

        /// <summary>
        /// On death, gives all nearby enemies +{bonus}% move speed
        /// </summary>
        public AvengerAbility Avenger { get; set; }

        /// <summary>
        /// Reduce every attack by {stoneskin}
        /// </summary>
        public int? Stoneskin { get; set; }


        public class FractureAbility
        {
            public FractureAbility(int count, MobType shard)
            {
                this.Count = count;
                this.Shard = shard;
            }

            public int Count { get; private set; }

            public MobType Shard { get; private set; }
        }

        public class AvengerAbility
        {
            public AvengerAbility(double range, double bonus)
            {
                this.Range = range;
                this.Bonus = bonus;
            }

            public double Range { get; private set; }

            public double Bonus { get; private set; }
        }
    }
}