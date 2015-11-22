using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tdd.Models
{
    [Serializable]
    public class Point
    {
        public double X { get; set; }

        public double Y { get; set; }

        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public static bool isNear(Point p1, Point p2, double distance)
        {
            var dx = p1.X - p2.X;
            var dy = p1.Y - p2.Y;

            return (dx * dx + dy * dy) < distance * distance;
        }
    }
}