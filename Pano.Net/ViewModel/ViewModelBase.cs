using System.ComponentModel;

namespace Pano.Net.ViewModel
{
    /// <summary>
    /// Base class for ViewModels
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
