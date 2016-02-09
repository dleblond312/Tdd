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

        public Point(Point p)
        {
            this.X = p.X;
            this.Y = p.Y;
        }

        public static bool IsNear(Point p1, Point p2, double distance)
        {
            var dx = p1.X - p2.X;
            var dy = p1.Y - p2.Y;

            return (dx * dx + dy * dy) <= distance * distance;
        }

        /// <summary>
        /// Updates a point value based on a straight line destination and a velocity.
        /// </summary>
        /// <param name="p1">The initial point to perform the operation on</param>
        /// <param name="p2">The destionation</param>
        /// <param name="velocity">The velocity to be applied</param>
        public static Point TrackTo(Point p1, Point p2, double velocity)
        {
            double x, y;

            if (p1.X == p2.X)
            {
                x = p1.X;
            }
            else if (p1.X < p2.X)
            {
                x = Math.Min(p1.X + velocity, p2.X);
            }
            else
            {
                x = Math.Max(p1.X - velocity, p2.X);
            }

            if (p1.Y == p2.Y)
            {
                y = p1.Y;
            }
            else if (p1.Y < p2.Y)
            {
                y = Math.Min(p1.Y + velocity, p2.Y);
            }
            else
            {
                y = Math.Max(p1.Y - velocity, p2.Y);
            }

            return new Point(x, y);
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