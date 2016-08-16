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
using CoreAnimation;

namespace MovieExplorer.iOS.UILayer.Controls
{
    public class MovieCell : UICollectionViewCell
    {
        public static readonly string CELL_ID = "MovieCellId";
        public static readonly int LOADING_CELL_ID = -1;
        UIImageView _imageView;
        UIActivityIndicatorView _activityIndicatorView;
        UIView _loadingView;

        [Export("initWithFrame:")]
        public MovieCell(CGRect frame) : base(frame)
        {
            Alpha = 0;
            BackgroundColor = UIColor.White;

            _imageView = new UIImageView(frame.AddMargin(1.0f));
            _imageView.Image = UIImage.FromBundle("Assets/Placeholder.png");
            _imageView.Center = frame.Reset().GetCenter();
            AddSubview(_imageView);

            Animate(0.2d, animation: () =>
            {
                Alpha = 1.0f;
            });


            //Loading View
            _loadingView = new UIView(frame.Reset());

            _activityIndicatorView = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.White);

            _activityIndicatorView.Center = frame.Reset().GetCenter();
            _loadingView.AddSubview(_activityIndicatorView);

            var loadingLabel = new BoldLabel(new CGRect(), "Loading...");
            loadingLabel.SizeToFit();
            loadingLabel.Frame = new CGRect(
                _activityIndicatorView.Center.X - loadingLabel.Frame.Width / 2,
                _activityIndicatorView.Frame.Bottom + MovieExplorerAppearance.DEFAULT_MARGIN,
                loadingLabel.Frame.Width,
                loadingLabel.Frame.Height);
            _loadingView.AddSubview(loadingLabel);

            AddSubview(_loadingView);
        }

        public async void SetMovie(Movie movie)
        {
            _loadingView.Alpha = 0.0f;
            BackgroundColor = UIColor.White;
            _activityIndicatorView.StopAnimating();
            var posterUrl = movie.PosterPath;
            if (!ImageCache.Instance.IsCached(posterUrl))
            {
                _imageView.Image = UIImage.FromBundle("Assets/Placeholder.png");
            }
            var uiImage = await ImageCache.Instance.GetOrDownloadImage(posterUrl);
            _imageView.Image = uiImage;
        }

        public void SetLoadingCell()
        {
            _imageView.Image = null;
            BackgroundColor = MovieExplorerAppearance.MOVIE_EXPLORER_ORANGE;
            _activityIndicatorView.StartAnimating();
            _loadingView.Alpha = 1.0f;
        }

        public void AnimateIn()
        {
            Alpha = 0.0f;
            Animate(0.2d, () =>
            {
                Alpha = 1.0f;
            });
        }
    }
}
