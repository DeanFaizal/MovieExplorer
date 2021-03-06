﻿using Foundation;
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
            try
            {
                var httpClient = new HttpClient();
                var data = await httpClient.GetByteArrayAsync(imageUrl);
                return UIImage.LoadFromData(NSData.FromArray(data));
            }
            catch (Exception)
            {
            }
            return null;
        }
    }
}
