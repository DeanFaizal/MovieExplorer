using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using System.Linq;
using System.Threading.Tasks;

namespace MovieExplorer.iOS.UILayer.Controls
{
    public class MovieCell : UICollectionViewCell
    {
        UIImageView _imageView;
        UILabel _loadingLabel;

        private string _posterUrl;
        public string PosterUrl
        {
            set
            {
                _posterUrl = value;
                LoadImage(value);
            }
        }

        [Export("initWithFrame:")]
        public MovieCell(CGRect frame) : base(frame)
        {
            BackgroundColor = UIColor.Blue;
            _imageView = new UIImageView(frame);
            _loadingLabel = new UILabel();
            _loadingLabel.Text = "Loading...";
            _loadingLabel.Center = frame.GetCenter();
            AddSubview(_loadingLabel);
        }
        private async Task LoadImage(string posterUrl)
        {
            var uiImage = await posterUrl.LoadImageFromUrl();
            if (Subviews.Contains(_loadingLabel))
            {
                _loadingLabel.RemoveFromSuperview();
            }
            _imageView.Image = uiImage;
            if (!Subviews.Contains(_imageView))
            {
                AddSubview(_imageView);
            }
        }        
    }
}
