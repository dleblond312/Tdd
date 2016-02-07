using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tdd.Models.Pathing;
using Tdd.Services;

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

            return (dx * dx + dy * dy) <= distance * distance;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Point p = obj as Point;
            return p.X == this.X && p.Y == this.Y;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return  (int) (Math.Pow(this.X, 2) + this.Y);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "<{0}, {1}>", this.X, this.Y);
        }
    }
}