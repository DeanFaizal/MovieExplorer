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
    public class MainVC : UIViewController
    {
        readonly string[] LIST_TITLES = { "Top Rated", "Popular", "Now Playing" };
        Dictionary<string, Func<Task<List<Movie>>>> _movieApiDictionary;
        Dictionary<string, List<Movie>> _movieListDictionary = new Dictionary<string, List<Movie>>();
        Dictionary<string, HorizontalMovieScroller> _movieScrollers = new Dictionary<string, HorizontalMovieScroller>();

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            InitializeNavBar();
            Initialize();
        }

        void InitializeNavBar()
        {
            NavigationController.NavigationBar.BarTintColor = MovieExplorerAppearance.MOVIE_EXPLORER_DARK_GRAY;

            Title = "Movie Explorer";
            NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes
            {
                ForegroundColor = MovieExplorerAppearance.MOVIE_EXPLORER_ORANGE,
                Font = MovieExplorerAppearance.NavBarTitleFont            };
        }

        async void Initialize()
        {
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
            for (int i = 0; i < movieListCount; i++)
            {
                await LoadMoviesForList(LIST_TITLES[i]);
            }
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
