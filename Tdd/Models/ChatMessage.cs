using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tdd.Models
{
    public class ChatMessage
    {

        public string Author { get; set; }
        public string Message { get; set; }
        public string SenderType { get; set; }
    }
}