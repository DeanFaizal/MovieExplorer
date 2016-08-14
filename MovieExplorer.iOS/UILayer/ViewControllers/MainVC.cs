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

namespace MovieExplorer.iOS.UILayer.ViewControllers
{
    public class MainVC : BaseViewController
    {
        readonly string[] LIST_TITLES = { "Top Rated", "Popular", "Now Playing" };

        Dictionary<string, Func<int, Task<List<Movie>>>> _movieApiDictionary;

        Dictionary<string, int> _movieListPageNumberDictionary;

        Dictionary<string, HorizontalMovieScrollerView> _movieScrollDictionary = new Dictionary<string, HorizontalMovieScrollerView>();

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
                var movieListCount = LIST_TITLES.Length;

                //Initialize dictionaries
                _movieApiDictionary = new Dictionary<string, Func<int, Task<List<Movie>>>>
                {
                    { LIST_TITLES[0], (page)=>MovieAccessor.Instance.GetTopRated(page) },
                    { LIST_TITLES[1], (page)=>MovieAccessor.Instance.GetPopular(page) },
                    { LIST_TITLES[2], (page)=>MovieAccessor.Instance.GetNowPlaying(page) }
                };

                _movieListPageNumberDictionary = new Dictionary<string, int>()
                {
                    {LIST_TITLES[0], 1 },
                    {LIST_TITLES[1], 1 },
                    {LIST_TITLES[2], 1 }
                };

                //Initialize UI
                var contentFrame = View.Frame.AddTopMargin(MovieExplorerAppearance.STATUS_BAR_HEIGHT + MovieExplorerAppearance.NAVIGATION_BAR_HEIGHT).AddBottomMargin();
                var verticalFrames = contentFrame.DivideVertical(movieListCount);
                for (int i = 0; i < movieListCount; i++)
                {
                    var horizontalMovieScroller = new HorizontalMovieScrollerView(verticalFrames[i], LIST_TITLES[i]);
                    horizontalMovieScroller.MovieSelected += OnMovieSelected;
                    horizontalMovieScroller.NextPageRequested += OnNextPageRequested;
                    _movieScrollDictionary.Add(LIST_TITLES[i], horizontalMovieScroller);
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
                alert.AddButton("Retry");
                alert.AddButton("OK");
                alert.Clicked += Alert_Clicked;
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
                var horizontalMovieScroller = _movieScrollDictionary[LIST_TITLES[i]];
                var movies = await _movieApiDictionary[LIST_TITLES[i]](1);
                horizontalMovieScroller.AddMovies(movies);
                }
            }
            catch (Exception)
            {
            }
        }

        private void Alert_Clicked(object sender, UIButtonEventArgs e)
        {
            if (e.ButtonIndex == 0)
            {

            }
        }

        private void OnMovieSelected(object sender, Movie selectedMovie)
        {
            var movieDetailsVC = new MovieDetailsVC(selectedMovie);
            NavigationController.PushViewController(movieDetailsVC, animated: true);
        }

        private async void OnNextPageRequested(object sender, string listTitle)
        {
            try
            {
                var nextPage = _movieListPageNumberDictionary[listTitle] + 1;
                _movieListPageNumberDictionary[listTitle] = nextPage;

                var movies = await _movieApiDictionary[listTitle](nextPage);
                _movieScrollDictionary[listTitle].AddMovies(movies);
            }
            catch (Exception)
            {
                var alert = new UIAlertView()
                {
                    Title = "An error occurred",
                    Message = string.Format("Failed to load more {0} movies", listTitle)
                };
                alert.AddButton("OK");
                alert.Show();
            }
        }
    }
}
