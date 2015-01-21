using System.Collections.ObjectModel;

namespace Pano.Net.Model
{

    /// <summary>
    /// Manager class for the list of recent images
    /// </summary>
    public class RecentImageManager
    {
        private const int MAXIMAGES = 10;

        public ObservableCollection<string> RecentImages { get; private set; }

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

        public void AddAndSave(string path)
        {
            if (RecentImages.Contains(path)) RecentImages.Remove(path);
            while (RecentImages.Count >= MAXIMAGES) RecentImages.RemoveAt(RecentImages.Count - 1);
            RecentImages.Insert(0, path);
            Properties.Settings.Default.RecentImages.Clear();
            foreach (string s in RecentImages) Properties.Settings.Default.RecentImages.Add(s);
            Properties.Settings.Default.Save();

        }
    }
}
