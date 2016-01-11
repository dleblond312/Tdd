using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tdd.Models
{
    [Serializable]
    public class BuildTowerCommand
    {
        public string Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

    }
}