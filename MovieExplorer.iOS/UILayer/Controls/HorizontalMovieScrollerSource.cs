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
        List<Movie> _movies;

        public event EventHandler<Movie> MovieSelected;

        public List<Movie> Movies
        {
            set { _movies = value; }
        }

        public HorizontalMovieScrollerSource(List<Movie> movies)
        {
            _movies = movies;
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
            movieCell.SetMovie(movie);
            return movieCell;
        }
        
        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var movie = _movies[indexPath.Row];
            MovieSelected?.Invoke(this, movie);
        }
    }
}
