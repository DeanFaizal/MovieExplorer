using CoreGraphics;
using MovieExplorer.Core.ServiceLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace MovieExplorer.iOS.UILayer.Controls
{
    public class HorizontalMovieScroller : UIView
    {
        public event EventHandler<Movie> MovieSelected;
        UICollectionView _movieCollectionView;
        HorizontalMovieScrollerSource _movieCollectionSource;

        public HorizontalMovieScroller(CGRect frame, string title, List<Movie> movies = null) : base(frame)
        {
            frame = frame.Reset();
            var titleLabel = new BoldLabel(frame, title);
            titleLabel.SizeToFit();
            titleLabel.Frame = titleLabel.Frame.SetX(MovieExplorerAppearance.DEFAULT_MARGIN).SetY(MovieExplorerAppearance.DEFAULT_MARGIN);
            AddSubview(titleLabel);

            var movieCollectionViewFrame = frame.AddTopMargin(titleLabel.Frame.Bottom + MovieExplorerAppearance.HALF_MARGIN);
            var itemHeight = movieCollectionViewFrame.Height;
            var itemWidth = MovieExplorerAppearance.POSTER_WIDTH_TO_HEIGHT_RATIO * itemHeight;

            _movieCollectionView = new UICollectionView(movieCollectionViewFrame, new UICollectionViewFlowLayout
            {
                ItemSize = new CGSize(itemWidth, itemHeight),
                ScrollDirection = UICollectionViewScrollDirection.Horizontal
            });
            _movieCollectionView.BackgroundColor = UIColor.Clear;
            _movieCollectionView.ShowsHorizontalScrollIndicator = false;
            _movieCollectionView.ShowsVerticalScrollIndicator = false;

            _movieCollectionView.ContentInset = new UIEdgeInsets(
                top: 0.0f,
                left: MovieExplorerAppearance.DEFAULT_MARGIN,
                bottom: 0.0f,
                right: MovieExplorerAppearance.DEFAULT_MARGIN);
            _movieCollectionView.RegisterClassForCell(typeof(MovieCell), MovieCell.CELL_ID);
            _movieCollectionSource = new HorizontalMovieScrollerSource(movies);
            _movieCollectionSource.MovieSelected += (sender, movie) =>
            {
                MovieSelected?.Invoke(this, movie);
            };
            _movieCollectionView.Source = _movieCollectionSource;
            AddSubview(_movieCollectionView);
        }

        public async Task LoadMovies(Func<Task<List<Movie>>> loadTask)
        {
            Movies = await loadTask();
        }

        public List<Movie> Movies
        {
            set
            {
                _movieCollectionSource.Movies = value;
                _movieCollectionView.ReloadData();
            }
        }
    }
}
