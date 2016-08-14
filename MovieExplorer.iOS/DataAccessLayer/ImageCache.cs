using MovieExplorer.Core.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using MovieExplorer.iOS.UILayer;

namespace MovieExplorer.iOS.DataAccessLayer
{
    public class ImageCache
    {
        private Dictionary<string, UIImage> _imageDictionary = new Dictionary<string, UIImage>();

        private static Lazy<ImageCache> _instance = new Lazy<ImageCache>();

        public static ImageCache Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public bool IsCached(string imagePath)
        {
            return _imageDictionary.ContainsKey(imagePath);
        }

        public async Task<UIImage> GetOrDownloadImage(string imageUrl)
        {
            UIImage image = null;
            if (!string.IsNullOrEmpty(imageUrl))
            {
                if (_imageDictionary.ContainsKey(imageUrl))
                {
                    image = _imageDictionary[imageUrl];
                }
                else
                {
                    image = await imageUrl.LoadImageFromUrl().ConfigureAwait(false);
                    if (!_imageDictionary.ContainsKey(imageUrl))
                    {
                        _imageDictionary.Add(imageUrl, image);
                    }
                }
            }
            return image;
        }
    }
}
