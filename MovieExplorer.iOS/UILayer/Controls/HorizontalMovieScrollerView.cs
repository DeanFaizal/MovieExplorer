﻿using CoreGraphics;
using MovieExplorer.Core.ServiceLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using UIKit;
using System.Linq;
using MovieExplorer.Core.ServiceAccessLayer;
using MovieExplorer.Core.BusinessLayer;

namespace MovieExplorer.iOS.UILayer.Controls
{
    public class HorizontalMovieScrollerView : UIView
    {
        public event EventHandler<Movie> MovieSelected;
        public event EventHandler<MovieListType> NextPageRequested;

        UICollectionView _movieCollectionView;
        MovieCollectionViewSource _movieCollectionViewSource;

        List<Movie> _movies = new List<Movie>();
        MovieListType _movieListType;
        bool _isLoading;
        UIActivityIndicatorView _loadingIndicatorView;
        UILabel _noResultsLabel;

        public HorizontalMovieScrollerView(CGRect frame, MovieListType movieListType, bool showLoading = true, bool loadMore = true) : base(frame)
        {
            _isLoading = showLoading;
            _movieListType = movieListType;


            //Initialize UI
            frame = frame.Reset();
            var titleLabel = new BoldLabel(frame, movieListType.GetDescription());
            titleLabel.SizeToFit();
            titleLabel.Frame = titleLabel.Frame.SetX(MovieExplorerAppearance.DEFAULT_MARGIN).SetY(MovieExplorerAppearance.DEFAULT_MARGIN);
            AddSubview(titleLabel);

            var movieCollectionViewFrame = frame.AddTopMargin(titleLabel.Frame.Bottom + MovieExplorerAppearance.HALF_MARGIN);

            InitializeLoadingView(movieCollectionViewFrame);
            InitializeNoResultsView(movieCollectionViewFrame);

            var itemHeight = movieCollectionViewFrame.Height;

            var itemWidth = MovieExplorerAppearance.POSTER_WIDTH_TO_HEIGHT_RATIO * itemHeight;

            _movieCollectionView = new UICollectionView(movieCollectionViewFrame, new UICollectionViewFlowLayout
            {
                ItemSize = new CGSize(itemWidth, itemHeight),
                ScrollDirection = UICollectionViewScrollDirection.Horizontal,
                MinimumInteritemSpacing = MovieExplorerAppearance.DEFAULT_MARGIN,
                MinimumLineSpacing = MovieExplorerAppearance.DEFAULT_MARGIN,
                SectionInset = new UIEdgeInsets(0, 0, 0, 0)
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
            _movieCollectionViewSource = new MovieCollectionViewSource(loadMore);
            _movieCollectionViewSource.MovieSelected += (sender, movie) =>
            {
                MovieSelected?.Invoke(this, movie);
            };
            _movieCollectionViewSource.NextPageRequested += async (sender, args) =>
            {
                await Task.Delay(1000); //slow down to avoid hitting API rate limit
                NextPageRequested?.Invoke(this, _movieListType);
            };
            _movieCollectionView.Source = _movieCollectionViewSource;
            AddSubview(_movieCollectionView);
        }

        private void InitializeLoadingView(CGRect frame)
        {
            _loadingIndicatorView = new UIActivityIndicatorView(frame);
            _loadingIndicatorView.ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.WhiteLarge;
            _loadingIndicatorView.HidesWhenStopped = true;
            if (_isLoading)
            {
                _loadingIndicatorView.StartAnimating();
            }
            AddSubview(_loadingIndicatorView);
        }

        private void InitializeNoResultsView(CGRect frame)
        {
            _noResultsLabel = new BoldLabel(frame, "No results");
            _noResultsLabel.SizeToFit();
            _noResultsLabel.Center = frame.GetCenter();
            _noResultsLabel.Hidden = true;

            AddSubview(_noResultsLabel);
        }

        public void AddMovies(List<Movie> movies)
        {
            if (_isLoading)
            {
                _isLoading = false;
                _loadingIndicatorView.StopAnimating();
            }
            else
            {
                _movieCollectionViewSource.StopAnimatingItemsIn(); //stop animating before reloading items to make sure loaded movies don't flicker
            }
            _movieCollectionViewSource.AddMovies(movies);
            _movieCollectionView.ReloadData();

            if (!_movieCollectionViewSource.HasMovies())
            {
                _noResultsLabel.Hidden = false;
            }
            else
            {
                _noResultsLabel.Hidden = true;
            }
        }

        public void ClearMovies()
        {
            _movieCollectionViewSource.ClearMovies();
            _movieCollectionView.ReloadData();
        }
    }
}
