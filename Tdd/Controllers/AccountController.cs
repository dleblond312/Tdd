using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Tdd.Models;

namespace Tdd.Controllers
{

    public class AccountController : Controller
    {
        private AuthRepository repo = null;

        public AccountController()
        {
            repo = new AuthRepository();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register()
        {
            Request.InputStream.Seek(0, SeekOrigin.Begin);
            string jsonData = new StreamReader(Request.InputStream).ReadToEnd();

            var userModel = JsonConvert.DeserializeObject<UserModel>(jsonData);


            if (!ModelState.IsValid)
            {
                throw new HttpException(400, "Bad Request");
            }

            var result = await repo.RegisterUser(userModel);

            if(result == null)
            {
                throw new HttpException(500, "Register Failed");
            }

            return Json("Success");
        }

        [HttpGet]
        public ActionResult Test()
        {
            return Json("Success");
        }
    }
}