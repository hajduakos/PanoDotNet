using System.Windows;

namespace Pano.Net
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            View.MainWindow mainWindow = new View.MainWindow();
            mainWindow.Show();
            if (e.Args.Length == 1) await (mainWindow.DataContext as ViewModel.MainViewModel).Open(e.Args[0]);
        }
    }
}
