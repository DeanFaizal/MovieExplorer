using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using UIKit;
using MovieExplorer.Core.ServiceLayer.Model;
using MovieExplorer.iOS.UILayer;
using System.Threading.Tasks;

namespace MovieExplorer.iOS.UILayer.Controls
{
    public class HorizontalMovieScrollerSource : UICollectionViewSource
    {
        readonly string _cellId;
        List<Movie> _movies;

        public event EventHandler<Movie> MovieSelected;

        public List<Movie> Movies
        {
            set { _movies = value; }
        }

        public HorizontalMovieScrollerSource(List<Movie> movies, string cellId)
        {
            _movies = movies;
            _cellId = cellId;
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
            var movieCell = (MovieCell)collectionView.DequeueReusableCell(_cellId, indexPath);
            
            var movie = _movies[indexPath.Row];
            movieCell.PosterUrl = movie.PosterPath;
            return movieCell;
        }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var movie = _movies[indexPath.Row];
            MovieSelected?.Invoke(this, movie);
        }
    }
}
