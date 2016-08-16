using MovieExplorer.Core.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using MovieExplorer.iOS.UILayer;
using System.Collections.Concurrent;
using MovieExplorer.Core.ServiceAccessLayer;

namespace MovieExplorer.iOS.DataAccessLayer
{
    public class ImageCache
    {
        private readonly int MAX_IMAGE_COUNT = 40;
        private ConcurrentQueue<string> _imageDictionaryKeyQueue = new ConcurrentQueue<string>(); //Keep track of the most recent urls
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
            if (IsValidImagePath(imagePath))
            {
                return _imageDictionary.ContainsKey(imagePath);
            }
            else
            {
                return false;
            }
        }

        public async Task<UIImage> GetOrDownloadImage(string imageUrl)
        {
            try
            {
                UIImage image = null;
                if (IsValidImagePath(imageUrl))
                {
                    if (_imageDictionary.ContainsKey(imageUrl))
                    {
                        image = _imageDictionary[imageUrl];
                    }
                    else
                    {
                        image = await imageUrl.LoadImageFromUrl().ConfigureAwait(false);
                        if (!_imageDictionary.ContainsKey(imageUrl) && image != default(UIImage)) //if the image isn't in the cache
                        {
                            var addSuccess = _imageDictionary.TryAdd(imageUrl, image); //add it
                            if (addSuccess)
                            {
                                _imageDictionaryKeyQueue.Enqueue(imageUrl); //add the key to the queue
                                if (_imageDictionaryKeyQueue.Count > MAX_IMAGE_COUNT) //if the queue is full
                                {
                                    var keyToRemove = string.Empty;
                                    if (_imageDictionaryKeyQueue.TryDequeue(out keyToRemove)) //dequeue the oldest key
                                    {
                                        UIImage removedImage;
                                        _imageDictionary.TryRemove(keyToRemove, out removedImage); //and remove it
                                    }
                                }
                            }
                        }
                    }
                }
                return image;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private bool IsValidImagePath(string imagePath)
        {
            return !string.IsNullOrEmpty(imagePath) && !imagePath.Equals(MovieAccessor.Instance.DEFAULT_POSTER_PATH);
        }
    }
}
