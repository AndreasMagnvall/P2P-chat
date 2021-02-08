using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TDDD49.Exceptions;

namespace TDDD49.Models
{
    class MessageParser
    {
        public static string ToJson<T>(T message)
        {
            return JsonConvert.SerializeObject(message, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }
        public static T FromJson<T>(string message)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(message, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
            }
            catch
            {
                throw new MessageParseException("Parsing failed");
            }
        }
    }
}
