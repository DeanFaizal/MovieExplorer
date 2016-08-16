using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using UIKit;
using MovieExplorer.Core.ServiceLayer.Model;
using MovieExplorer.iOS.UILayer;
using System.Threading.Tasks;
using System.Linq;

namespace MovieExplorer.iOS.UILayer.Controls
{
    public class MovieCollectionViewSource : UICollectionViewSource
    {
        public event EventHandler<Movie> MovieSelected;
        public event EventHandler NextPageRequested;
                
        List<Movie> _movies = new List<Movie>();
        bool _loadMore = true;
        bool _isAnimating = true;

        public List<Movie> Movies
        {
            get
            {
                return _movies;
            }
        }

        public MovieCollectionViewSource(bool loadMore = true)
        {
            _loadMore = loadMore;
        }

        public void AddMovies(List<Movie> movies)
        {
            //remove loading cell
            var loadingCell = _movies.FirstOrDefault(a => a.Id == MovieCell.LOADING_CELL_ID);
            if (loadingCell != null)
            {
                _movies.Remove(loadingCell);
            }
            else
            {
                loadingCell = new Movie { Id = MovieCell.LOADING_CELL_ID };
            }

            if (movies != null && movies.Count != 0) //don't load more if there aren't any movies left
            {
                _movies.AddRange(movies);
                if (_loadMore)
                {
                    _movies.Add(loadingCell); //add the loading cell to the end to trigger infinite scroll
                }
            }
        }

        public bool HasMovies()
        {
            return _movies.Any();
        }

        public void ClearMovies()
        {
            _movies.Clear();
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            if (_movies == null)
            {
                return 0;
            }
            else
            {
                return _movies.Count;
            }
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var movieCell = (MovieCell)collectionView.DequeueReusableCell(MovieCell.CELL_ID, indexPath);

            var movie = _movies[indexPath.Row];

            if (movie.Id == MovieCell.LOADING_CELL_ID)
            {
                NextPageRequested?.Invoke(this, null);
                movieCell.SetLoadingCell();
            }
            else
            {
                movieCell.SetMovie(movie);
            }
            if (_isAnimating)
            {
                movieCell.AnimateIn();
            }
            return movieCell;
        }

        public override void Scrolled(UIScrollView scrollView)
        {
            _isAnimating = true; //Start animating again on scrolled
        }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var movie = _movies[indexPath.Row];
            if (movie.Id == MovieCell.LOADING_CELL_ID)
            {
                var alert = new UIAlertView()
                {
                    Title = "Loading...",
                    Message = string.Format("Loading more movies")
                };
                alert.AddButton("OK");
                alert.Show();
            }
            else
            {
                MovieSelected?.Invoke(this, movie);
            }
        }

        /// <summary>
        /// Stop animating when more movies are added so that the movies don't flicker in place
        /// </summary>
        public void StopAnimatingItemsIn()
        {
            _isAnimating = false;
        }
    }
}
