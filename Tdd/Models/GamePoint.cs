using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tdd.Models.Pathing;

namespace Tdd.Models
{
    public class GamePoint : Point, IHasNeighbours<GamePoint>
    {
        private readonly GameRoom GameRoom;

        public GamePoint(GameRoom gameRoom, double x, double y) : base(x, y)
        {
            this.GameRoom = gameRoom;
        }

        public GamePoint(GameRoom gameRoom, Point p) : base(p.X, p.Y)
        {
            this.GameRoom = gameRoom;
        }

        [JsonIgnore]
        public IEnumerable<GamePoint> Neighbours
        {

            get
            {
                var list = new List<GamePoint>();

                var upPoint = new GamePoint(this.GameRoom, (int)X, (int)Y - 1);
                var up = this.GameRoom.Towers.Where(x => x.Location == upPoint).Any();
                if(!up)
                {
                    list.Add(new GamePoint(this.GameRoom, upPoint));
                }

                var leftPoint = new GamePoint(this.GameRoom, (int)X - 1, (int)Y);
                var left = this.GameRoom.Towers.Where(x => x.Location == leftPoint).Any();
                if(!left)
                {
                    list.Add(new GamePoint(this.GameRoom, leftPoint));
                }

                var rightPoint = new GamePoint(this.GameRoom, (int)X + 1, (int)Y);
                var right = this.GameRoom.Towers.Where(x => x.Location == rightPoint).Any();
                if(!right)
                {
                    list.Add(new GamePoint(this.GameRoom, rightPoint));
                }

                var downPoint = new GamePoint(this.GameRoom, (int)X, (int)Y+1);
                var down = this.GameRoom.Towers.Where(x => Point.isNear(this, x.Location, 0.5)).Any();
                if(!down)
                {
                    list.Add(new GamePoint(this.GameRoom, downPoint));
                }

                return list;
            }
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