using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using Tdd.Models.Pathing;

namespace Tdd.Models
{
    public class GamePoint : Point, IHasNeighbours<GamePoint>
    {
        private const double MaxDistance = 0.5;
        private readonly GameRoom GameRoom;

        public GamePoint(GameRoom gameRoom, double x, double y) : base(Math.Max(Math.Min(x, Constants.MapSize), 0), Math.Max(Math.Min(y, Constants.MapSize), 0))
        {
            this.GameRoom = gameRoom;
        }

        public GamePoint(GameRoom gameRoom, Point p) : this(gameRoom, (int)p.X, (int)p.Y)  { }

        [JsonIgnore]
        public IEnumerable<GamePoint> Neighbours
        {

            get
            {
                var intX = (int)this.X;
                var intY = (int)this.Y;
                var comparePoint = new Point(intX, intY);
                var list = new List<GamePoint>();

                comparePoint.Y = intY - 1;
                if(intY - 1 > 0 && !this.GameRoom.Towers.ContainsKey(comparePoint))
                {
                    list.Add(new GamePoint(this.GameRoom, intX, intY - 1));
                }

                comparePoint.Y = intY + 1;
                if(intY + 1 <= Constants.MapSize && !this.GameRoom.Towers.ContainsKey(comparePoint))
                {
                    list.Add(new GamePoint(this.GameRoom, intX, intY + 1));
                }

                comparePoint.X = intX - 1;
                comparePoint.Y = intY;
                if(intX - 1 > 0 && !this.GameRoom.Towers.ContainsKey(comparePoint))
                {
                    list.Add(new GamePoint(this.GameRoom, intX - 1, intY));
                }

                comparePoint.X = intX + 1;
                comparePoint.Y = intY;
                if (intX + 1 <= Constants.MapSize && !this.GameRoom.Towers.ContainsKey(comparePoint))
                {
                    list.Add(new GamePoint(this.GameRoom, intX + 1, intY));
                }


                // Game boundary limits
                return list.Where(p => 
                    (
                        (p.X >= Constants.MapThird && p.X < Constants.MapThird * 2) || 
                        (p.Y >= Constants.MapThird && p.Y < Constants.MapThird * 2)
                    ));

                //return list.Where(p => 
                //    (p.X >= Constants.MapThird && p.X <= Constants.MapThird*2 && p.Y >= 0 && p.Y <= Constants.MapSize) ||
                //    (p.Y >= Constants.MapThird && p.Y <= Constants.MapThird*2 && p.X >= 0&& p.X <= Constants.MapSize));
            }
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return (int)(Math.Pow(this.X, 2) + this.Y);
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Point p = obj as Point;
            return (int)p.X == (int)this.X && (int)p.Y == (int)this.Y;
        }
    }
}