using Foundation;
using MovieExplorer.Core.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace MovieExplorer.iOS.UILayer
{
    public static class UIImageExtensions
    {
        public static async Task<UIImage> LoadImageFromUrl(this string imageUrl)
        {
            var httpClient = new HttpClient();
            var data = await httpClient.GetByteArrayAsync(imageUrl);
            return UIImage.LoadFromData(NSData.FromArray(data));
        }

        public static async Task<UIImage> LoadImageFromCache(this string imageUrl)
        {
            UIImage image;
            try
            {
                image = await LocalCache.Instance.Load<UIImage>(imageUrl);
            }
            catch (Exception ex)
            {
                image = await imageUrl.LoadImageFromUrl();
                await LocalCache.Instance.Save(imageUrl, image);
            }
            return image;
        }
    }
}
