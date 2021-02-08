using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDDD49.Models;
using TDDD49.Models.Messages;

namespace TDDD49.Database
{
    public class DatabaseManager
    {
        public static string path;
        public static string WriteImage(ImageDataMessage image, ConversationInfo info)
        {
            string imagePath = info.ImagePath;
            string fullpath = imagePath + image.extension;
            Console.WriteLine(fullpath);
            File.WriteAllBytes(fullpath, image.ImageBytes());
            return fullpath;
        }

        public static ConversationInfo ReadInfo(string path)
        {
            string info = File.ReadAllText(path);
            ConversationInfo conversationInfo = (ConversationInfo)JsonConvert.DeserializeObject(info);
            return conversationInfo;
        }

        public static void WriteInfo(ConversationInfo info)
        {
            string infoPath = info.InfoPath;
            string infoStr = JsonConvert.SerializeObject(info);
            File.WriteAllText(infoPath, infoStr);
        }

        public static void WriteMessages(List<Message> messages, ConversationInfo info)
        {
            // Directory.CreateDirectory(fullpath);

            string messagesText = JsonConvert.SerializeObject(messages, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            Console.WriteLine(info.MessagesPath);
            File.WriteAllText(info.MessagesPath, messagesText);
        }

        public static List<Message> ReadMessages(ConversationInfo info)
        {
            string messages = File.ReadAllText(info.MessagesPath);

            List<Message> msgs = (List<Message>)JsonConvert.DeserializeObject(messages, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            return msgs;
        }
    }
}
