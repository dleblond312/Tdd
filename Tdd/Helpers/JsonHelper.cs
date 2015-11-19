using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tdd.Helpers
{
    public static class JsonHelper
    {
        private static JsonSerializerSettings SerializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

        public static string Serialize(object o)
        {
            return JsonConvert.SerializeObject(o, SerializerSettings);
        }
    }
}