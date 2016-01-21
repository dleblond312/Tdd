using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Tdd.Models
{
    public class Constants
    {
        public static readonly int MapSize = 500;
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

        public static readonly List<MobType> MobTypes = new List<MobType>()
        {
            new MobType()
            {
                MoveSpeed = 1000,
                StartingHealth = 30
            }
        };

        public static readonly int RoundSize = 10;
        public static readonly int RoundPauseMs = 1000;
        public static readonly int RoundUpdateMs = 10;
        public static readonly int GameSpeed = 50;

        public static readonly Resources StartingResources = new Resources()
        {
            Primary = 100,
            Income = 10
        };

        public static readonly IList<TowerType> TowerTypes = new List<TowerType>()
        {
            new TowerType()
            {
                Id = 1,
                Text = "Simple Tower",
                Damage = 10,
                Speed = 100
            },
            new TowerType()
            {
                Id = 2,
                Text = "Slow Tower",
                Damage = 20,
                Speed = 175
            }
        };
    }
}