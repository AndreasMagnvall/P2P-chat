using System.ComponentModel;

namespace TDDD49.ViewModels
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string connected)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(connected.ToString()));
        }
    }
}
