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
using System.Runtime.CompilerServices;
using Reachability;
using MovieExplorer.Core.BusinessLayer;
using CBZSplashViewBinding;

namespace MovieExplorer.iOS.UILayer.ViewControllers
{
    public class MainVC : BaseViewController
    {
        readonly MovieListType[] MOVIE_LIST_TYPES = {
            MovieListType.TopRated,
            MovieListType.Popular,
            MovieListType.NowPlaying
        };

        Dictionary<MovieListType, Func<int, Task<List<Movie>>>> _movieApiDictionary;

        Dictionary<MovieListType, int> _movieListPageNumberDictionary;

        Dictionary<MovieListType, HorizontalMovieScrollerView> _movieScrollDictionary;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Initialize();
        }

        private async void Initialize()
        {
            try
            {           
                Title = "Movie Explorer";

                View.BackgroundColor = MovieExplorerAppearance.MOVIE_EXPLORER_LIGHT_GRAY;
                var movieListCount = MOVIE_LIST_TYPES.Length;

                //Initialize dictionaries
                _movieApiDictionary = new Dictionary<MovieListType, Func<int, Task<List<Movie>>>>
                {
                    { MOVIE_LIST_TYPES[0], (page)=>MovieAccessor.Instance.GetTopRated(page) },
                    { MOVIE_LIST_TYPES[1], (page)=>MovieAccessor.Instance.GetPopular(page) },
                    { MOVIE_LIST_TYPES[2], (page)=>MovieAccessor.Instance.GetNowPlaying(page) }
                };

                _movieListPageNumberDictionary = new Dictionary<MovieListType, int>()
                {
                    {MOVIE_LIST_TYPES[0], 1 },
                    {MOVIE_LIST_TYPES[1], 1 },
                    {MOVIE_LIST_TYPES[2], 1 }
                };

                _movieScrollDictionary = new Dictionary<MovieListType, HorizontalMovieScrollerView>();


                //Initialize UI
                var contentFrame = View.Frame.AddTopMargin(MovieExplorerAppearance.STATUS_BAR_HEIGHT + MovieExplorerAppearance.NAVIGATION_BAR_HEIGHT).AddBottomMargin();
                var verticalFrames = contentFrame.DivideVertical(movieListCount);
                for (int i = 0; i < movieListCount; i++)
                {
                    var horizontalMovieScroller = new HorizontalMovieScrollerView(verticalFrames[i], MOVIE_LIST_TYPES[i]);
                    horizontalMovieScroller.MovieSelected += OnMovieSelected;
                    horizontalMovieScroller.NextPageRequested += OnNextPageRequested;
                    _movieScrollDictionary.Add(MOVIE_LIST_TYPES[i], horizontalMovieScroller);
                    View.AddSubview(horizontalMovieScroller);
                }

                await InitializeMovies(movieListCount);
            }
            catch (Exception ex)
            {
                var alert = new UIAlertView()
                {
                    Title = "An error occurred",
                    Message = ex.Message
                };
                alert.AddButton("OK");
                alert.Show();
            }

            if (Reachability.Reachability.InternetConnectionStatus() == NetworkStatus.NotReachable)
            {

                var alert = new UIAlertView()
                {
                    Title = "No network connection",
                    Message = "Please check your network settings and restart the app"
                };
                alert.AddButton("OK");
                alert.Show();
            }
        }

        private async Task InitializeMovies(int movieListCount)
        {
            try
            {
                //Then load the movies
                for (int i = 0; i < movieListCount; i++)
                {
                    var horizontalMovieScroller = _movieScrollDictionary[MOVIE_LIST_TYPES[i]];
                    var movies = await _movieApiDictionary[MOVIE_LIST_TYPES[i]](1);
                    horizontalMovieScroller.AddMovies(movies);
                }
            }
            catch (Exception ex)
            {
                var alert = new UIAlertView()
                {
                    Title = "An error occurred",
                    Message = ex.Message
                };
                alert.AddButton("OK");
                alert.Show();
            }
        }

        private void OnMovieSelected(object sender, Movie selectedMovie)
        {
            var movieDetailsVC = new MovieDetailsVC(selectedMovie);
            NavigationController.PushViewController(movieDetailsVC, animated: true);
        }

        private async void OnNextPageRequested(object sender, MovieListType movieList)
        {
            try
            {
                var nextPage = _movieListPageNumberDictionary[movieList] + 1;
                _movieListPageNumberDictionary[movieList] = nextPage;

                var movies = await _movieApiDictionary[movieList](nextPage);
                _movieScrollDictionary[movieList].AddMovies(movies);
            }
            catch (Exception)
            {
                var alert = new UIAlertView()
                {
                    Title = "An error occurred",
                    Message = string.Format("Failed to load more {0} movies", movieList)
                };
                alert.AddButton("OK");
                alert.Show();
            }
        }
    }
}
