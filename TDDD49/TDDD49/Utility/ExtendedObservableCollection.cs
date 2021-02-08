using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49.Models
{
    public class ExtendedObservableCollection<T> : ObservableCollection<T>
    {
        public void ReplaceAll<K> (K collection) where K : ICollection<T> 
        {
            this.Clear();
            foreach (var info in collection)
            {
                this.Add(info);
            }
        }
        public void Sort<K>(Func<T, K> func)
        {
            var sortedList = Items.OrderByDescending(func).ToList();
            

            for (int i = 0; i < sortedList.Count; ++i)
            {
                var actualItemIndex = Items.IndexOf(sortedList[i]);

                if (actualItemIndex != i)
                    Move(actualItemIndex, i);
            }
        }
    }
}
