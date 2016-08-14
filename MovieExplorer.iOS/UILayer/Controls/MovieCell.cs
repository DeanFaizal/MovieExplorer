using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using System.Linq;
using System.Threading.Tasks;
using MovieExplorer.Core.ServiceLayer.Model;
using MovieExplorer.Core.DataAccessLayer;
using MovieExplorer.iOS.DataAccessLayer;

namespace MovieExplorer.iOS.UILayer.Controls
{
    public class MovieCell : UICollectionViewCell
    {
        public static readonly string CELL_ID = "MovieCellId";
        UIImageView _imageView;

        [Export("initWithFrame:")]
        public MovieCell(CGRect frame) : base(frame)
        {
            Alpha = 0;
            BackgroundColor = UIColor.White;

            _imageView = new UIImageView(frame.AddMargin(1.0f));
            _imageView.Image = UIImage.FromBundle("Assets/Placeholder.png");
            _imageView.Center = frame.GetCenter();
            AddSubview(_imageView);

            Animate(0.2d, animation: () =>
            {
                Alpha = 1.0f;
            });
        }

        public async void SetMovie(Movie movie)
        {
            var posterUrl = movie.PosterPath;
            if (!ImageCache.Instance.IsCached(posterUrl))
            {
                _imageView.Image = UIImage.FromBundle("Assets/Placeholder.png");
            }
            var uiImage = await ImageCache.Instance.GetOrDownloadImage(posterUrl);

            if (uiImage != null)
            {
                Alpha = 0.0f;
                _imageView.Image = uiImage;
                Animate(0.2d, () =>
                {
                    Alpha = 1.0f;
                });
            }
        }
    }
}
