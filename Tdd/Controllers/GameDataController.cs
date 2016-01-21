using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tdd.Models;

namespace Tdd.Controllers
{
    public class GameDataController : Controller
    {

        [HttpGet]
        [AllowAnonymous]
        public ContentResult GetTowerTypes()
        {
            var data = Constants.TowerTypes;

            return new ContentResult()
            {
                Content = JsonConvert.SerializeObject(data),
                ContentType = "application/json"
            };
        }
    }
}