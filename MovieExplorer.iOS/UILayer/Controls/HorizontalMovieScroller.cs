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
            var titleLabel = new UILabel();
            titleLabel.Text = title;
            titleLabel.SizeToFit();
            AddSubview(titleLabel);

            _movieCollectionView = new UICollectionView(frame, new UICollectionViewFlowLayout {
                ItemSize = MovieExplorerAppearance.POSTER_SIZE,
                MinimumInteritemSpacing = 0.0f,
                ScrollDirection = UICollectionViewScrollDirection.Horizontal });
            var cellId = "Cell";
            _movieCollectionView.RegisterClassForCell(typeof(MovieCell), cellId);
            _movieCollectionSource = new HorizontalMovieScrollerSource(movies, cellId);
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
