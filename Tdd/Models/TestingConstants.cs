using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tdd.Models
{
    public class TestingConstants
    {

        public static List<Tower> Maze = new List<Tower>
        {
            new TestingTower(15, 11),
            new TestingTower(16, 11),
            new TestingTower(17, 11),
            new TestingTower(18, 11),
            new TestingTower(19, 11),
            new TestingTower(20, 11),
            new TestingTower(21, 11),
            new TestingTower(22, 11),
            new TestingTower(23, 11),
            new TestingTower(24, 11),
            new TestingTower(25, 11),
            //new TestingTower(26, 11),
            //new TestingTower(27, 11),
            //new TestingTower(28, 11),

            //new TestingTower(16, 14),
            //new TestingTower(17, 14),
            //new TestingTower(18, 14),
            //new TestingTower(19, 14),
            //new TestingTower(20, 14),
            //new TestingTower(21, 14),
            //new TestingTower(22, 14),
            //new TestingTower(23, 14),
            //new TestingTower(24, 14),
            //new TestingTower(25, 14),
            //new TestingTower(26, 14),
            //new TestingTower(27, 14),
            //new TestingTower(28, 14),
            //new TestingTower(29, 14),
        };
    }


    public class TestingTower : Tower
    {
        private static int TowerId = 0;

        public TestingTower(int x, int y) : base(Constants.TowerTypes[0], "Testing Tower", new Point(x, y), TowerId++.ToString())
        {
        }
    }
}