using System.Collections.ObjectModel;

namespace Pano.Net.Model
{

    /// <summary>
    /// Manager class for the list of recent images
    /// </summary>
    public class RecentImageManager
    {
        private const int MAXIMAGES = 10;

        /// <summary>
        /// List of recent images
        /// </summary>
        public ObservableCollection<string> RecentImages { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public RecentImageManager()
        {
            RecentImages = new ObservableCollection<string>();
            if (Properties.Settings.Default.RecentImages == null)
            {
                Properties.Settings.Default.RecentImages = new System.Collections.Specialized.StringCollection();
            }
            foreach (string s in Properties.Settings.Default.RecentImages)
            {
                RecentImages.Add(s);
            }
        }

        /// <summary>
        /// Add image to the list and save list
        /// </summary>
        /// <param name="path">Path of the image to be added</param>
        public void AddAndSave(string path)
        {
            // Remove to avoid duplicates
            if (RecentImages.Contains(path)) RecentImages.Remove(path);
            // Remove if the list is full
            while (RecentImages.Count >= MAXIMAGES) RecentImages.RemoveAt(RecentImages.Count - 1);
            // Add to list and save
            RecentImages.Insert(0, path);
            Properties.Settings.Default.RecentImages.Clear();
            foreach (string s in RecentImages) Properties.Settings.Default.RecentImages.Add(s);
            Properties.Settings.Default.Save();

        }
    }
}
