using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Tdd.Models
{
    public class Constants
    {
        public static readonly int MapSizeX = 32;
        public static readonly int MapSizeY = 7;
        public static readonly int StartingLife = 30;
        public static readonly double InterestRate = 1.04;

        public static readonly List<Point> StartingLocations = new List<Point>()
        {
            new Point(Constants.MapSizeX/2, 0),
            new Point(Constants.MapSizeX/2, Constants.MapSizeY),
        };

        public static readonly List<Point> EndingLocations = new List<Point>()
        {
            new Point(Constants.MapSizeX/2, Constants.MapSizeY),
            new Point(Constants.MapSizeX/2, 0),
        };

        public static readonly List<MobType> MobTypes = new List<MobType>()
        {
            new MobType()
            {
                MoveSpeed = 200,
                StartingHealth = 200,
                Abilities = new Abilities()
                {
                    Rewards = new Abilities.RewardAbility()
                    {
                        Income = 1,
                        Primary = 1
                    }
                }
            },
            new MobType()
            {
                MoveSpeed = 200,
                StartingHealth = 275,
                Abilities = new Abilities()
                {
                    Rewards = new Abilities.RewardAbility()
                    {
                        Income = 1,
                        Primary = 2
                    }
                }
            }, 
            new MobType()
            {
                MoveSpeed = 400,
                StartingHealth = 250,
                Abilities = new Abilities()
                {
                    Rewards = new Abilities.RewardAbility()
                    {
                        Income = 1,
                        Primary = 2
                    }
                }
            },
            new MobType()
            {
                MoveSpeed = 300,
                StartingHealth = 250,
                Abilities = new Abilities()
                {
                    Evasion = 25,
                    Rewards = new Abilities.RewardAbility()
                    {
                        Income = 2,
                        Primary = 2
                    }
                }
            },
            new MobType()
            {
                MoveSpeed = 100,
                StartingHealth = 1750,
                Abilities = new Abilities()
                {
                    Evasion = 5,
                    Stoneskin = 2,
                    Rewards = new Abilities.RewardAbility()
                    {
                        Income = 3,
                        Primary = 25
                    }
                }
            }
        };

        public static readonly int RoundSize = 10;
        public static readonly int RoundPauseMs = 1000;
        public static readonly int RoundUpdateMs = 10;
        public static readonly int GameSpeed = 100000;

        public static readonly Resources StartingResources = new Resources()
        {
            Primary = 150,
            Income = 10
        };

        public enum TowerList
        {
            Rock,
            Basic,
            Slowing,
            LongRange,
            Dot,
            Melee,
        }

        public static readonly IList<TowerType> TowerTypes = new List<TowerType>()
        {
            new TowerType()
            {
                Id = TowerList.Rock,
                Text = "Rock",
                Cost = new Cost()
                {
                    Primary = 2
                }
            },
            new TowerType()
            {
                Id = TowerList.Basic,
                Text = "Basic Tower",
                Damage = 10,
                Range = 3,
                Speed = 1000,
                ProjectileSpeed = 1000,
                Cost = new Cost()
                {
                    Primary = 10
                }
            },
            new TowerType()
            {
                Id = TowerList.Slowing,
                Text = "Slowing Tower",
                Damage = 6,
                Range = 4,
                Speed = 1000,
                ProjectileSpeed = 1000,
                Effects = JsonConvert.DeserializeObject("{'slow': { speed: 0.7, duration: 3000 }}"),
                Cost = new Cost()
                {
                    Primary = 20
                }
            },
            new TowerType()
            {
                Id = TowerList.LongRange,
                Text = "Long Range Tower",
                Damage = 15,
                Range = 5,
                Speed = 800,
                ProjectileSpeed = 1000,
                Cost = new Cost()
                {
                    Primary = 20
                }
            },
            new TowerType()
            {
                Id = TowerList.Dot,
                Text = "Damage Over Time Tower",
                Damage = 2,
                Range = 4,
                Speed = 1750,
                ProjectileSpeed = 1000,
                Effects = JsonConvert.DeserializeObject("{'dot': { damage: 2, duration: 20000}}"),
                Cost = new Cost()
                {
                    Primary = 24
                }
            }, 
            new TowerType()
            {
                Id = TowerList.Melee,
                Text = "Melee Tower",
                Damage = 20,
                Range = 1,
                Speed = 2000,
                ProjectileSpeed = 1000,
                Cost = new Cost()
                {
                    Primary = 26
                }
            }
        };
    }
}