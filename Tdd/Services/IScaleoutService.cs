using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tdd.Models;

namespace Tdd.Services
{
    public interface IScaleoutService
    {
        void Store(string name, object o);

        object Get(string name);
    }
}
