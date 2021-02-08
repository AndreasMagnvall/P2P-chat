using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TDDD49.Models;
using TDDD49.Models.Services;

namespace TDDD49.Models
{
    public class ConversationInfo
    {
        private readonly string myUsername;
        private readonly string peerUsername;
        public readonly string date;
        private readonly string basePath;

        public string MyUsername   { get { return myUsername; } }
        public string PeerUsername { get { return peerUsername; } }
        public string Date         { get { return date; } }

        public string MessagesPath { 
            get
            {
                return basePath + @"\messages.json";
            }
        }
        public string ImagePath { 
        
            get
            { 
                return ImagesPath + Guid.NewGuid().ToString(); //random 

            }      
        }

        public string ImagesPath
        {

            get
            {
                return basePath + @"\images\";

            }
        }

        public string InfoPath
        {
            get
            {
                return basePath + @"\info.json";
            }         
        }

        public ConversationInfo(string myUsername, string peerUsername, string date)
        {
            this.myUsername = myUsername;
            this.peerUsername = peerUsername;
            this.date = date; //DateTime.Now.ToString();
            string currentDir = Directory.GetCurrentDirectory();
            string conversations = @"\conversations\"; 
            //string fullpath = currentDir + conversations;
            string folderPath = currentDir + conversations;



            this.basePath = folderPath + myUsername + "_" + peerUsername + "_"; //+ date.Trim();
            //this.messagesPath = @"\messages.json";
            Console.WriteLine(basePath);
            Directory.CreateDirectory(basePath);
            Directory.CreateDirectory(ImagesPath);
        }
    }
}
