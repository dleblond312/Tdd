using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tdd.Models;
using Tdd.Models.Pathing;

namespace Tdd.Services
{
    public interface IPathingService
    {

        Path<Node> FindPath<Node>(Node start, Node destination, Func<Node, Node, double> distance, Func<Node, double> estimate) where Node : IHasNeighbours<Node>;
    }
}
