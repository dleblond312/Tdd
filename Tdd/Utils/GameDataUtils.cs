using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tdd.Models;

namespace Tdd.Utils
{
    public static class GameDataUtils
    {
        public static TowerType GetTowerTypeFromTowerList(Constants.TowerList towerObj)
        {
            return Constants.TowerTypes.Where(t => t.Id == towerObj).First();
        }
    }
}