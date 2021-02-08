using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace TDDD49.Utility
{
    public class Runner {
        public static void Primary(Action action) 
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => action()));
        }
        public static Thread Secondary(Action action)
        {
            Thread thread = new Thread(new ThreadStart(() => action()))
            {
                IsBackground = true
            };
            thread.Start();
            return thread;
        }
    }
}
