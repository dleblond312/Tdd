using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdd.Models.Pathing
{
    public interface IHasNeighbours<N>
    {
        IEnumerable<N> Neighbours { get; }
    }
}
