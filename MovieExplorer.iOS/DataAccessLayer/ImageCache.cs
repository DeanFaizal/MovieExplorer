using MovieExplorer.Core.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using MovieExplorer.iOS.UILayer;
using System.Collections.Concurrent;

namespace MovieExplorer.iOS.DataAccessLayer
{
    public class ImageCache
    {
        private ConcurrentDictionary<string, UIImage> _imageDictionary = new ConcurrentDictionary<string, UIImage>();

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
                        _imageDictionary.TryAdd(imageUrl, image);
                    }
                }
            }
            return image;
        }
    }
}
