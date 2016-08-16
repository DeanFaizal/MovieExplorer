using CoreGraphics;
using Foundation;
using MovieExplorer.Core.ServiceAccessLayer;
using MovieExplorer.Core.ServiceLayer.Model;
using MovieExplorer.iOS.UILayer.Controls;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading;
using System.Text;
using System.Threading;
using UIKit;

namespace MovieExplorer.iOS.UILayer.ViewControllers
{
	public class SearchVC : BaseViewController
	{
		IDisposable _searchQuerySubscription;
		HorizontalMovieScrollerView _searchResultsScrollerView;

		public override async void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			Initialize ();
		}

		async void Initialize ()
		{
			View.BackgroundColor = MovieExplorerAppearance.MOVIE_EXPLORER_LIGHT_GRAY;

			var searchBoxWidth = 240;
			var searchBoxFrame = new CGRect (
				View.Frame.Width / 2 - searchBoxWidth / 2,
				MovieExplorerAppearance.TOTAL_TOP_BAR_HEIGHT + MovieExplorerAppearance.DEFAULT_MARGIN,
				searchBoxWidth,
				31);

			var searchBox = new UISearchBar (searchBoxFrame);
			searchBox.SearchBarStyle = UISearchBarStyle.Minimal;
			searchBox.BarStyle = UIBarStyle.Default;
			searchBox.TintColor = MovieExplorerAppearance.MOVIE_EXPLORER_ORANGE;
			var searchBoxTextView = (UITextField)searchBox.ValueForKey (new NSString ("searchField"));
			searchBoxTextView.TextColor = UIColor.White;
			searchBox.Placeholder = "Search";
			searchBox.SearchButtonClicked += (sender, e) => {
				searchBox.ResignFirstResponder ();
			};
			View.AddSubview (searchBox);

			View.AddGestureRecognizer (new UITapGestureRecognizer (() => {
				searchBox.ResignFirstResponder ();
			}));


			var searchTextChangedObservable = Observable.FromEventPattern<UISearchBarTextChangedEventArgs> (
				a => searchBox.TextChanged += a,
				a => searchBox.TextChanged -= a);
			_searchQuerySubscription = searchTextChangedObservable.Throttle (TimeSpan.FromSeconds (.5)).ObserveOn (SynchronizationContext.Current).Subscribe (async e => {
				try {
					var searchQuery = e.EventArgs.SearchText;
					var movies = await MovieAccessor.Instance.Search (searchQuery);
					_searchResultsScrollerView.ClearMovies ();
					_searchResultsScrollerView.AddMovies (movies);
				} catch (Exception ex) {
					_searchResultsScrollerView.ClearMovies ();
				}
			});


			var resultsViewFrame = new CGRect (0,
				searchBoxFrame.Bottom + MovieExplorerAppearance.DEFAULT_MARGIN,
				View.Frame.Width,
				View.Frame.Height - searchBoxFrame.Bottom - MovieExplorerAppearance.DEFAULT_MARGIN * 2);
			_searchResultsScrollerView = new HorizontalMovieScrollerView (resultsViewFrame, MovieListType.SearchResults, showLoading: false, loadMore: false);
			_searchResultsScrollerView.MovieSelected += SearchResultsScrollerViewMovieSelected;
			View.AddSubview (_searchResultsScrollerView);

			NavigationItem.SetRightBarButtonItem (new UIBarButtonItem (UIBarButtonSystemItem.Done, (sender, args) => {
				DismissModalViewController (animated: true);
			}), animated: true);
		}

		private void SearchResultsScrollerViewMovieSelected (object sender, Movie selectedMovie)
		{
			var movieDetailsVC = new MovieDetailsVC (selectedMovie);
			NavigationController.PushViewController (movieDetailsVC, animated: true);
		}
	}
}
