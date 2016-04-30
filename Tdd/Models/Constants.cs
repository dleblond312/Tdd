using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Tdd.Models
{
    public class Constants
    {
        public static readonly int MapSize = 45;
        public static readonly int MapThird = MapSize / 3;
        public static readonly int StartingLife = 30;

        public static readonly List<Point> StartingLocations = new List<Point>()
        {
            new Point(Constants.MapSize/2,0),
            new Point(Constants.MapSize, Constants.MapSize/2),
            new Point(Constants.MapSize/2, Constants.MapSize),
            new Point(0, Constants.MapSize/2)
        };

        public static readonly List<Point> EndingLocations = new List<Point>()
        {
            new Point(Constants.MapSize/2, Constants.MapSize),
            new Point(0, Constants.MapSize/2),
            new Point(Constants.MapSize/2,0),
            new Point(Constants.MapSize, Constants.MapSize/2),
        };

        public static IList<Point> Map = new List<Point>()
        {
            new Point(0, MapThird),
            new Point(MapThird, MapThird),
            new Point(MapThird, 0),
            new Point(MapThird * 2, 0),
            new Point(MapThird * 2, MapThird),
            new Point(MapSize, MapThird),
            new Point(MapSize, MapThird * 2),
            new Point(MapThird * 2, MapThird * 2),
            new Point(MapThird * 2, MapSize),
            new Point(MapThird, MapSize),
            new Point(MapThird, MapThird * 2),
            new Point(0, MapThird * 2),
            new Point(0, MapThird)
        };

        public static readonly List<MobType> MobTypes = new List<MobType>()
        {
            new MobType()
            {
                MoveSpeed = 500,
                StartingHealth = 500
            }
        };

        public static readonly int RoundSize = 10;
        public static readonly int RoundPauseMs = 1000;
        public static readonly int RoundUpdateMs = 10;
        public static readonly int GameSpeed = 100000;

        public static readonly Resources StartingResources = new Resources()
        {
            Primary = 10000,
            Income = 10
        };

        public enum TowerList {
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
                ProjectileSpeed = 2000,
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
                ProjectileSpeed = 2000,
                Effects = @"{'slow': { speed: 0.7, duration: 3000 }}",
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
                Effects = @"{'dot': { damage: 1, duration: 20000}}",
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
                Cost = new Cost()
                {
                    Primary = 26
                }
            }
        };
    }
}