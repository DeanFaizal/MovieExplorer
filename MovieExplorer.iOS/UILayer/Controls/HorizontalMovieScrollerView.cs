using CoreGraphics;
using MovieExplorer.Core.ServiceLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using UIKit;
using System.Linq;

namespace MovieExplorer.iOS.UILayer.Controls
{
    public class HorizontalMovieScrollerView : UIView
    {
        public event EventHandler<Movie> MovieSelected;
        public event EventHandler<string> NextPageRequested;

        UICollectionView _movieCollectionView;
        MovieCollectionViewSource _movieCollectionViewSource;

        List<Movie> _movies = new List<Movie>();

        public HorizontalMovieScrollerView(CGRect frame, string title) : base(frame)
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
            _movieCollectionViewSource = new MovieCollectionViewSource();
            _movieCollectionViewSource.MovieSelected += (sender, movie) =>
            {
                MovieSelected?.Invoke(this, movie);
            };
            _movieCollectionViewSource.NextPageRequested += (sender, args) =>
            {
                NextPageRequested?.Invoke(this, title);
            };
            _movieCollectionView.Source = _movieCollectionViewSource;
            AddSubview(_movieCollectionView);
        }
        
        public void AddMovies(List<Movie> movies)
        {
            _movieCollectionViewSource.AddMovies(movies);            
            _movieCollectionView.ReloadData();
        }
    }
}
