using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Tdd.Models
{
    [Serializable]
    public class GameRoom
    {
        public int Id { get; set; }

        public IList<IPrincipal> Users { get; set; }


        public GameRoom()
        {
        }
    }
}