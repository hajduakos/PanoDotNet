using Pano.Net.Model;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace Pano.Net.View
{
    /// <summary>
    /// Class for viewing an equirectangular panorama by projecting it on the
    /// inner surface of a sphere
    /// </summary>
    public partial class PanoView : UserControl, INotifyPropertyChanged
    {
        private MeshGeometry3D sphereMesh = null; // Tessellated sphere mesh
        private ImageBrush brush = null;          // Brush containing the panorama
        private double camTheta = 180;            // Camera horizontal orientation
        private double camPhi = 90;               // Camera vertical orientation
        private double camThetaSpeed = 0;         // Camera horizontal movement speed
        private double camPhiSpeed = 0;           // Camera vertical movement speed
        private double clickX, clickY;            // Coordinates of the mouse press
        private DispatcherTimer timer;            // Timer for animating camera
        private bool isMouseDown = false;         // Is the mouse pressed

        /// <summary>
        /// Camera horizontal FOV
        /// </summary>
        public double Hfov { get { return MyCam.FieldOfView; } }

        /// <summary>
        /// Camera vertical FOV
        /// </summary>
        public double Vfov { get { return MyCam.FieldOfView * ActualHeight / ActualWidth; } }

        /// <summary>
        /// Camera horizontal orientation
        /// </summary>
        public double Theta { get { return camTheta; } }

        /// <summary>
        /// Camera vertical orientation
        /// </summary>
        public double Phi { get { return camPhi; } }

        /// <summary>
        /// Constructor
        /// </summary>
        public PanoView()
        {
            InitializeComponent();
            sphereMesh = GeometryHelper.CreateSphereMesh(40, 20, 10); // Initialize mesh 
           
            brush = new ImageBrush(); // Initialize brush with no image
            brush.TileMode = TileMode.Tile;
            
            timer = new DispatcherTimer(); // Initialize timer
            timer.Interval = TimeSpan.FromMilliseconds(25);
            timer.Tick += timer_Tick;
        }

        /// <summary>
        /// Dependency property for the panorama
        /// </summary>
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(BitmapImage), typeof(PanoView),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, ImageChangedCallback));

        // Callback for image changed
        private static void ImageChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PanoView)d).ImageChanged();
        }

        /// <summary>
        /// Panorama
        /// </summary>
        public BitmapImage Image
        {
            get { return (BitmapImage)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        // Image changed
        private void ImageChanged()
        {
            MyModel.Children.Clear();
            brush.ImageSource = Image;

            ModelVisual3D sphereModel = new ModelVisual3D();
            sphereModel.Content = new GeometryModel3D(sphereMesh, new DiffuseMaterial(brush));
            MyModel.Children.Add(sphereModel);
            
            RaisePropertyChanged("Hfov");
            RaisePropertyChanged("Vfov");
        }

        // Timer: animate camera
        private void timer_Tick(object sender, EventArgs e)
        {
            if (!isMouseDown) return;
            camTheta += camThetaSpeed / 50;
            camPhi += camPhiSpeed / 50;

            if (camTheta < 0) camTheta += 360;
            else if (camTheta > 360) camTheta -= 360;

            if (camPhi < 0.01) camPhi = 0.01;
            else if (camPhi > 179.99) camPhi = 179.99;

            MyCam.LookDirection = GeometryHelper.GetNormal(
                GeometryHelper.Deg2Rad(camTheta),
                GeometryHelper.Deg2Rad(camPhi));

            RaisePropertyChanged("Theta");
            RaisePropertyChanged("Phi");
        }

        // Mouse move: set camera movement speed
        private void vp_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isMouseDown) return;
            camThetaSpeed = Mouse.GetPosition(vp).X - clickX;
            camPhiSpeed = Mouse.GetPosition(vp).Y - clickY;
        }

        // Mouse down: start moving camera
        private void vp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = true;
            this.Cursor = Cursors.SizeAll;
            clickX = Mouse.GetPosition(vp).X;
            clickY = Mouse.GetPosition(vp).Y;
            camThetaSpeed = camPhiSpeed = 0;
            timer.Start();
        }

        // Mouse up: stop moving camera
        private void vp_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = false;
            this.Cursor = Cursors.Arrow;
            timer.Stop();
        }

        // Mouse wheel: zoom
        private void vp_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            MyCam.FieldOfView -= e.Delta / 100;
            if (MyCam.FieldOfView < 1) MyCam.FieldOfView = 1;
            else if (MyCam.FieldOfView > 140) MyCam.FieldOfView = 140;

            RaisePropertyChanged("Hfov");
            RaisePropertyChanged("Vfov");
        }

        // Size changed: notify FOV change
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            RaisePropertyChanged("Hfov");
            RaisePropertyChanged("Vfov");
        }

        // Helper function for INPC
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
