using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using MovieExplorer.iOS.UILayer;
using CoreGraphics;
using MovieExplorer.iOS.UILayer.Controls;
using MovieExplorer.Core.ServiceLayer.Model;
using MovieExplorer.Core.ServiceAccessLayer;

namespace MovieExplorer.iOS.UILayer.ViewControllers
{
    public class MainVC : BaseViewController
    {
        readonly string[] LIST_TITLES = { "Top Rated", "Popular", "Now Playing" };
        Dictionary<string, Func<Task<List<Movie>>>> _movieApiDictionary;
        Dictionary<string, List<Movie>> _movieListDictionary = new Dictionary<string, List<Movie>>();
        Dictionary<string, HorizontalMovieScroller> _movieScrollers = new Dictionary<string, HorizontalMovieScroller>();

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Initialize();
        }
        
        async void Initialize()
        {
            Title = "Movie Explorer";

            View.BackgroundColor = MovieExplorerAppearance.MOVIE_EXPLORER_LIGHT_GRAY;
            var movieListCount = LIST_TITLES.Length;

            _movieApiDictionary = new Dictionary<string, Func<Task<List<Movie>>>>
            {
                { LIST_TITLES[0], ()=>MovieAccessor.Instance.GetTopRated() },
                { LIST_TITLES[1], ()=>MovieAccessor.Instance.GetPopular() },
                { LIST_TITLES[2], ()=>MovieAccessor.Instance.GetNowPlaying() }
            };

            //Initialize UI
            var contentFrame = View.Frame.AddTopMargin(MovieExplorerAppearance.STATUS_BAR_HEIGHT + MovieExplorerAppearance.NAVIGATION_BAR_HEIGHT).AddBottomMargin();
            var verticalFrames = contentFrame.DivideVertical(movieListCount);
            for (int i = 0; i < movieListCount; i++)
            {
                var horizontalMovieScroller = new HorizontalMovieScroller(verticalFrames[i], LIST_TITLES[i]);
                horizontalMovieScroller.MovieSelected += OnMovieSelected;
                _movieScrollers.Add(LIST_TITLES[i], horizontalMovieScroller);
                View.AddSubview(horizontalMovieScroller);
            }

            //Then load the movies
            var tasks = new Task[movieListCount];
            for (int i = 0; i < movieListCount; i++)
            {
                tasks[i] = LoadMoviesForList(LIST_TITLES[i]);
            }

            await Task.WhenAll(tasks);
        }

        async Task LoadMoviesForList(string listTitle)
        {
            var movies = await _movieApiDictionary[listTitle].Invoke();
            _movieScrollers[listTitle].Movies = movies;
        }

        void OnMovieSelected(object sender, Movie selectedMovie)
        {
            var movieDetailsVC = new MovieDetailsVC(selectedMovie);
            NavigationController.PushViewController(movieDetailsVC, animated: true);
        }
    }
}
