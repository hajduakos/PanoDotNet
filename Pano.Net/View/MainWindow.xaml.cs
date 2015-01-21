using Microsoft.Win32;
using Pano.Net.ViewModel;
using System.ComponentModel;
using System.Windows;

namespace Pano.Net.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();

            // If the window style is set to none when the window is maximized, the taskbar will not
            // be covered. Therefore, the window is restored to normal and maximized again.
            DependencyPropertyDescriptor d = DependencyPropertyDescriptor.FromProperty(
                Window.WindowStyleProperty,typeof(Window));
            d.AddValueChanged(this, (sender, args) =>
            {
                Window w = (Window)sender;
                if (w.WindowStyle == System.Windows.WindowStyle.None)
                {
                    w.WindowState = System.Windows.WindowState.Normal;
                    w.WindowState = System.Windows.WindowState.Maximized;
                }
            });

        }        
    }
}
