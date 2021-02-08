using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TDDD49.Commands;
using TDDD49.Database;

namespace TDDD49.Models.Services
{
    public class ConversationsService: IService
    {
        private ExtendedObservableCollection<ConversationInfo> allConversations;

        public ExtendedObservableCollection<ConversationInfo> Conversations { get; set; }

        public ConversationsService()
        {
            allConversations = new ExtendedObservableCollection<ConversationInfo>();
            Conversations = new ExtendedObservableCollection<ConversationInfo>();     
        }

        public void SearchConversation(string username)
        {
            var queryConversations = from conversation in allConversations
                                     where conversation.PeerUsername.Contains(username) || conversation.MyUsername.Contains(username)
                                     select conversation;


            Conversations.ReplaceAll(queryConversations.ToList<ConversationInfo>());
            Conversations.Sort((i) => Convert.ToDateTime(i.date));
        }

        public void SetSearchResultToAll()
        {
            allConversations.Sort((i) => Convert.ToDateTime(i.date));

            Conversations.ReplaceAll(allConversations);
        }

        public void Start()
        {
            FetchConversations();
        }
        private async Task FetchConversations() {

            string currentDir = Directory.GetCurrentDirectory();
            string conversations = @"\conversations\"; 
            string folderPath = currentDir + conversations;
            string[] folders = Directory.GetDirectories(folderPath);
            

            foreach(string path in folders)
            {
                string infoStr = File.ReadAllText(path + @"\info.json"); //DatabaseManager.ReadPathContent(path);
                ConversationInfo conversationInfo = JsonConvert.DeserializeObject<ConversationInfo>(infoStr);
                allConversations.Add(conversationInfo);
                Console.WriteLine(allConversations.Count);
            }
            await Application.Current.Dispatcher.BeginInvoke(new Action(() => SetSearchResultToAll()));
            Console.WriteLine(Conversations.Count);
        }
    }
}
