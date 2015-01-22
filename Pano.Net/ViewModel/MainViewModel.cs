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

        /// <summary>
        /// Open image with dialog
        /// </summary>
        public ICommand OpenCommand { get; private set; }

        /// <summary>
        /// Open image by file name
        /// </summary>
        public ICommand OpenWithFilenameCommand { get; private set; }

        /// <summary>
        /// Exit application
        /// </summary>
        public ICommand ExitCommand { get; private set; }

        /// <summary>
        /// Toggle fullscreen
        /// </summary>
        public ICommand FullscreenCommand { get; private set; }

        /// <summary>
        /// Display controls
        /// </summary>
        public ICommand ControlsCommand { get; private set; }

        /// <summary>
        /// Display about information
        /// </summary>
        public ICommand AboutCommand { get; private set; }
        #endregion

        // Public properties
        #region public_properties

        /// <summary>
        /// Panorama
        /// </summary>
        public BitmapImage Image { get; private set; }

        /// <summary>
        /// Is fullscreen mode on
        /// </summary>
        public bool IsFullscreen { get; private set; }

        /// <summary>
        /// Is the model loading
        /// </summary>
        public bool IsLoading { get; private set; }

        /// <summary>
        /// Recent images manager
        /// </summary>
        public RecentImageManager RecentImageManager { get; private set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public MainViewModel()
        {
            OpenCommand = new RelayCommand(a => Open());
            OpenWithFilenameCommand = new RelayCommand(a => Open((string)a));
            ExitCommand = new RelayCommand(a => Exit());
            FullscreenCommand = new RelayCommand(a => FullScreen());
            ControlsCommand = new RelayCommand(a => Controls());
            AboutCommand = new RelayCommand(a => About());

            RecentImageManager = new Model.RecentImageManager(); RaisePropertyChanged("RecentImages");
            
            Image = null; RaisePropertyChanged("Image");
            IsFullscreen = false; RaisePropertyChanged("IsFullscreen");
            IsLoading = false; RaisePropertyChanged("IsLoading");

        }

        // Private methods
        #region private_methods

        // Open image with dialog
        private async void Open()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images (*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if (ofd.ShowDialog() == true)
            {
                await Open(ofd.FileName);
            }
        }

        // Open image by file name
        public async Task Open(string path)
        {
            Image = null; RaisePropertyChanged("Image");
            IsLoading = true; RaisePropertyChanged("IsLoading");

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

            IsLoading = false; RaisePropertyChanged("IsLoading");
            RaisePropertyChanged("Image");
        }

        // Exit application
        private void Exit()
        {
            App.Current.Shutdown();
        }

        // Toggle fullscreen
        private void FullScreen()
        {
            IsFullscreen = !IsFullscreen;
            RaisePropertyChanged("IsFullscreen");
        }

        //Display controls
        private void Controls()
        {
            InfoMessage("Controls", "Click and drag the mouse to move camera.\r\nScroll to zoom.");
        }

        // Display about information
        private void About()
        {
            InfoMessage("About", "Created by Ákos Hajdu, 2014.");
        }

        // Helper function to display an information
        private void InfoMessage(string caption, string text)
        {
            MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Helper function to display a warning
        private void WarningMessage(string caption, string text)
        {
            MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        #endregion
    }
}
