using Microsoft.Win32;
using Pano.Net.Commands;
using Pano.Net.Model;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Pano.Net.ViewModel
{
    /// <summary>
    /// Main ViewModel
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        // Commands
        #region commands
        public ICommand OpenCommand { get; private set; }
        public ICommand OpenWithFilenameCommand { get; private set; }
        public ICommand ExitCommand { get; private set; }
        public ICommand FullscreenCommand { get; private set; }
        public ICommand ControlsCommand { get; private set; }
        public ICommand AboutCommand { get; private set; }
        #endregion

        // Public properties
        #region public_properties
        public BitmapImage Image { get; private set; }
        public bool IsFullscreen { get; private set; }
        public bool IsLoading { get; private set; }
        public RecentImageManager RecentImageManager { get; private set; }
        #endregion

        public MainViewModel()
        {
            OpenCommand = new RelayCommand(a => Open());
            OpenWithFilenameCommand = new RelayCommand(a => Open((string)a));
            ExitCommand = new RelayCommand(a => Exit());
            FullscreenCommand = new RelayCommand(a => FullScreen());
            ControlsCommand = new RelayCommand(a => Controls());
            AboutCommand = new RelayCommand(a => About());

            RecentImageManager = new Model.RecentImageManager();

            RaisePropertyChanged("RecentImages");
            Image = null; 
            RaisePropertyChanged("Image");
            IsFullscreen = false; 
            RaisePropertyChanged("IsFullscreen");
            IsLoading = false; 
            RaisePropertyChanged("IsLoading");

        }

        // Private methods
        #region private_methods
        private async void Open()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images (*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if (ofd.ShowDialog() == true)
            {
                await Open(ofd.FileName);
            }
        }

        public async Task Open(string path)
        {
            Image = null;
            IsLoading = true;
            RaisePropertyChanged("Image");
            RaisePropertyChanged("IsLoading");


            await Task.Factory.StartNew(() =>
            {
                Image = new BitmapImage();
                Image.BeginInit();
                Image.CacheOption = BitmapCacheOption.OnLoad;
                Image.UriSource = new Uri(path);
                Image.EndInit();
                Image.Freeze();
            });

            if (Math.Abs(Image.Width / Image.Height - 2) > 0.001)
                WarningMessage("Warning", "The opened image is not equirectangular (2:1)! Rendering may be improper.");

            RecentImageManager.AddAndSave(path);

            IsLoading = false;
            RaisePropertyChanged("Image");
            RaisePropertyChanged("IsLoading");
        }

        private void Exit()
        {
            App.Current.Shutdown();
        }

        private void FullScreen()
        {
            IsFullscreen = !IsFullscreen;
            RaisePropertyChanged("IsFullscreen");
        }
        private void Controls()
        {
            InfoMessage("Controls", "Click and drag the mouse to move camera.\r\nScroll to zoom.");
        }

        private void About()
        {
            InfoMessage("About", "Created by Ákos Hajdu, 2014.");
        }

        private void InfoMessage(string caption, string text)
        {
            MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void WarningMessage(string caption, string text)
        {
            MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        #endregion
    }
}
