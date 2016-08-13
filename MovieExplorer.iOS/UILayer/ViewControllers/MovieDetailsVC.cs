using CoreGraphics;
using MovieExplorer.Core.DataAccessLayer;
using MovieExplorer.Core.ServiceAccessLayer;
using MovieExplorer.Core.ServiceLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using System.Linq;
using MovieExplorer.iOS.UILayer.Controls;
using Foundation;
using MediaPlayer;

namespace MovieExplorer.iOS.UILayer.ViewControllers
{
    public class MovieDetailsVC : UIViewController
    {
        Movie _movie;
        List<Video> _videos;
        MovieExplorerButton _playVideoButton;
        UIImageView _posterView;
        bool _isFavorite = false;

        UIButton _saveToFavoritesButton;

        public MovieDetailsVC(Movie movie)
        {
            _movie = movie;
        }

        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();
            await Initialize();
        }

        private async Task Initialize()
        {
            View.BackgroundColor = UIColor.White;

            var mainContentFrame = View.Frame.AddTopMargin(MovieExplorerAppearance.TOTAL_TOP_BAR_HEIGHT);

            var contentFrames = mainContentFrame.DivideVertical(3);

            var movieDetailsView = GenerateMovieDetailsView(contentFrames[0]);
            View.AddSubview(movieDetailsView);

            var movieDescriptionView = GenerateMovieDescriptionView(contentFrames[1]);
            View.AddSubview(movieDescriptionView);

            var similarMoviesView = GenerateSimilarMoviesView(contentFrames[2]);
            View.AddSubview(similarMoviesView);

            var loadPosterTask = LoadPoster();
            var updateFavoriteButtonTask = UpdateFavoriteButton();
            var loadSimilarMoviesTask = similarMoviesView.LoadMovies(async () =>
            {
                return await MovieAccessor.Instance.GetSimilar(_movie.Id);
            });
            var loadVideosTask = LoadVideos();

            await Task.WhenAll(loadPosterTask,
                updateFavoriteButtonTask,
                loadSimilarMoviesTask,
                loadVideosTask);
        }

        private UIView GenerateMovieDetailsView(CGRect frame)
        {
            var movieDetailsView = new UIView(frame);
            movieDetailsView.BackgroundColor = UIColor.Green;
            var movieDetailFrames = frame.Reset().AddDefaultMargin().DivideHorizontal(new float[2] { 1.0f, 2.0f });
            _posterView = new UIImageView(movieDetailFrames[0]);
            _posterView.BackgroundColor = UIColor.Purple;
            movieDetailsView.AddSubview(_posterView);

            var movieInfoView = GenerateMovieInfoView(movieDetailFrames[1]);
            movieDetailsView.AddSubview(movieInfoView);

            return movieDetailsView;
        }

        private async Task LoadPoster()
        {
            _posterView.Image = await _movie.PosterPath.LoadImageFromUrl();
        }

        private UIView GenerateMovieInfoView(CGRect frame)
        {
            var movieInfoView = new UIView(frame.AddLeftMargin(MovieExplorerAppearance.DEFAULT_MARGIN));
            movieInfoView.BackgroundColor = UIColor.Purple;

            //From top
            var titleLabel = new UILabel(new CGRect(0, 0, 1, 1));
            titleLabel.Text = _movie.Title;
            titleLabel.SizeToFit();
            movieInfoView.AddSubview(titleLabel);

            var releaseDateLabel = new UILabel(new CGRect(0, titleLabel.Frame.Bottom.AddDefaultMargin(), 1, 1));
            releaseDateLabel.Text = _movie.ReleaseDate;
            releaseDateLabel.SizeToFit();
            movieInfoView.AddSubview(releaseDateLabel);

            var ratingsViewOrigin = new CGPoint(0, releaseDateLabel.Frame.Bottom.AddDefaultMargin());
            var ratingsView = GenerateRatingsView(ratingsViewOrigin, _movie.VoteAverage);
            movieInfoView.AddSubview(ratingsView);

            //From bottom
            var saveToFavoritesButtonFrame = new CGRect(0, 0, 0.0f, MovieExplorerAppearance.DEFAULT_BUTTON_HEIGHT);
            _saveToFavoritesButton = new MovieExplorerButton(saveToFavoritesButtonFrame, color: UIColor.Orange, title: "Save to Favorites");
            _saveToFavoritesButton.TouchUpInside += async (sender, args) =>
            {
                if (_isFavorite)
                {
                    await FavoritesAccessor.Instance.RemoveFromFavorites(_movie?.Id.ToString());
                }
                else
                {
                    await FavoritesAccessor.Instance.AddToFavorites(_movie?.Id.ToString());
                }
                await UpdateFavoriteButton();
            };
            _saveToFavoritesButton.SizeToFit();
            _saveToFavoritesButton.Frame = _saveToFavoritesButton.Frame.SetY(movieInfoView.Frame.Bottom - _saveToFavoritesButton.Frame.Height - MovieExplorerAppearance.DEFAULT_MARGIN);
            movieInfoView.AddSubview(_saveToFavoritesButton);

            var playVideoButtonFrame = new CGRect(0, _saveToFavoritesButton.Frame.Top - MovieExplorerAppearance.DEFAULT_MARGIN / 2 - MovieExplorerAppearance.DEFAULT_BUTTON_HEIGHT, 1.0f, MovieExplorerAppearance.DEFAULT_BUTTON_HEIGHT);
            _playVideoButton = new MovieExplorerButton(playVideoButtonFrame, color: UIColor.Gray, title: "Loading Video...");
            
            _playVideoButton.TouchUpInside += (sender, args) =>
            {
                PlayVideo();
            };

            _playVideoButton.SizeToFit();
            _playVideoButton.Frame = _playVideoButton.Frame.SetY(_saveToFavoritesButton.Frame.Top - _playVideoButton.Frame.Height - MovieExplorerAppearance.SMALL_MARGIN);
            movieInfoView.AddSubview(_playVideoButton);
            
            return movieInfoView;
        }

        private UIView GenerateRatingsView(CGPoint origin, double ratingAverage)
        {
            var frame = new CGRect(origin, size: new CGSize());
            return new UIView();
        }

        private async Task UpdateFavoriteButton()
        {
            var movieId = _movie?.Id.ToString();
            _isFavorite = await FavoritesAccessor.Instance.IsFavorite(movieId);
            if (_isFavorite)
            {
                _saveToFavoritesButton.SetTitle("Remove from Favorites", UIControlState.Normal);
            }
            else
            {
                _saveToFavoritesButton.SetTitle("Save to Favorites", UIControlState.Normal);
            }
            _saveToFavoritesButton.SizeToFit();
        }

        private void PlayVideo()
        {
            if (_videos!=null)
            {
                var video = _videos.FirstOrDefault(a=>a.Site.ToLower().Equals("youtube"));
                if (video != null)
                {
                    UIApplication.SharedApplication.OpenUrl(NSUrl.FromString(string.Format("https://www.youtube.com/watch?v={0}", video.Key)));
                }
            }
        }

        private UIView GenerateMovieDescriptionView(CGRect frame)
        {
            var movieDescriptionView = new UIView(frame);
            movieDescriptionView.BackgroundColor = UIColor.Gray;
            frame = frame.Reset();

            var movieDescriptionLabel = new UILabel(frame.AddMargin(MovieExplorerAppearance.DEFAULT_MARGIN));
            movieDescriptionLabel.Lines = 0;
            movieDescriptionLabel.LineBreakMode = UILineBreakMode.WordWrap;
            movieDescriptionLabel.Text = _movie.Overview;
            movieDescriptionLabel.SizeToFit();

            var scrollView = new UIScrollView(frame);
            scrollView.ContentInset = new UIEdgeInsets(
                top: 0.0f,//MovieExplorerAppearance.DEFAULT_MARGIN,
                left: 0.0f,//MovieExplorerAppearance.DEFAULT_MARGIN,
                bottom: MovieExplorerAppearance.DEFAULT_MARGIN * 10.0f,
                right: 0.0f);// MovieExplorerAppearance.DEFAULT_MARGIN);
            scrollView.ContentSize = movieDescriptionLabel.Frame.Size;
            scrollView.AddSubview(movieDescriptionLabel);

            movieDescriptionView.AddSubview(scrollView);
            return movieDescriptionView;
        }

        private HorizontalMovieScroller GenerateSimilarMoviesView(CGRect frame)
        {
            var similarMoviesView = new HorizontalMovieScroller(frame, title: "Similar Movies");
            similarMoviesView.MovieSelected += (sender, selectedMovie) =>
            {
                var similarMovieDetailsVC = new MovieDetailsVC(selectedMovie);
                NavigationController.PushViewController(similarMovieDetailsVC, animated: true);
            };

            return similarMoviesView;
        }

        private async Task LoadVideos()
        {
            _videos = await MovieAccessor.Instance.GetVideos(_movie.Id);
            _playVideoButton.SetTitle("Play Video", UIControlState.Normal);
            _playVideoButton.SizeToFit();
            _playVideoButton.BackgroundColor = UIColor.Green;
        }
    }
}
