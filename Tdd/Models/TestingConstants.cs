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
            new TestingTower(25, 11),
            new TestingTower(26, 11),
            new TestingTower(27, 11),
            new TestingTower(28, 11),
            new TestingTower(29, 11),
            new TestingTower(30, 11),
            new TestingTower(31, 11),
            new TestingTower(32, 11),
            new TestingTower(33, 11),
            new TestingTower(34, 11),
            new TestingTower(35, 11),
            new TestingTower(36, 11),
            new TestingTower(37, 11),
            new TestingTower(38, 11),
            new TestingTower(39, 11),
            new TestingTower(40, 11),
            new TestingTower(41, 11),
            new TestingTower(42, 11),
            new TestingTower(43, 11),
            new TestingTower(44, 11),
            new TestingTower(45, 11),
            new TestingTower(46, 11),

            new TestingTower(49, 14),
            new TestingTower(48, 14),
            new TestingTower(47, 14),
            new TestingTower(46, 14),
            new TestingTower(45, 14),
            new TestingTower(44, 14),
            new TestingTower(43, 14),
            new TestingTower(42, 14),
            new TestingTower(41, 14),
            new TestingTower(40, 14),
            new TestingTower(39, 14),
            new TestingTower(38, 14),
            new TestingTower(37, 14),
            new TestingTower(36, 14),
            new TestingTower(35, 14),
            new TestingTower(34, 14),
            new TestingTower(33, 14),
            new TestingTower(32, 14),
            new TestingTower(31, 14),
            new TestingTower(30, 14),
            new TestingTower(29, 14),
            new TestingTower(28, 14),
            new TestingTower(27, 14)
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