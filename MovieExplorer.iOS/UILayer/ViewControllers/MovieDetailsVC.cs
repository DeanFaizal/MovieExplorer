﻿using CoreGraphics;
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
using MovieExplorer.iOS.DataAccessLayer;

namespace MovieExplorer.iOS.UILayer.ViewControllers
{
    public class MovieDetailsVC : BaseViewController
    {
        Movie _movie;
        List<Video> _videos;
        MovieExplorerButton _playVideoButton;
        UIImageView _posterImageView;
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
            var mainContentFrame = View.Frame.AddTopMargin(MovieExplorerAppearance.TOTAL_TOP_BAR_HEIGHT);


            //Background
            var backgroundView = await GenerateBackgroundView(mainContentFrame);
            View.AddSubview(backgroundView);

            //Content Frames
            var contentFrames = mainContentFrame.DivideVertical(3);

            //Movie details/dashboard
            var movieDetailsView = GenerateMovieDetailsView(contentFrames[0]);
            View.AddSubview(movieDetailsView);

            //Movie description
            var movieDescriptionView = GenerateMovieDescriptionView(contentFrames[1]);
            View.AddSubview(movieDescriptionView);

            //Similar movies
            var similarMoviesView = GenerateSimilarMoviesView(contentFrames[2]);
            View.AddSubview(similarMoviesView);

            //Load data
            await LoadData(similarMoviesView);
        }

        private async Task<UIView> GenerateBackgroundView(CGRect frame)
        {
            var backgroundImageView = new UIImageView(frame);
            backgroundImageView.ClipsToBounds = true;

            var backgroundImage = await ImageCache.Instance.GetOrDownloadImage(_movie.PosterPath);
            if (backgroundImage == null)
            {
                backgroundImage = UIImage.FromBundle("Assets/Placeholder.png");
            }
            backgroundImageView.ContentMode = UIViewContentMode.ScaleAspectFill;
            backgroundImageView.Image = backgroundImage;
            var blur = UIBlurEffect.FromStyle(UIBlurEffectStyle.Dark);
            var blurView = new UIVisualEffectView(blur)
            {
                Frame = frame.Reset()
            };

            backgroundImageView.Add(blurView);
            return backgroundImageView;
        }

        private async Task LoadData(HorizontalMovieScroller similarMoviesView)
        {
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
            var movieDetailFrames = frame.Reset().AddDefaultMargin().DivideHorizontal(new float[2] { 1.0f, 2.0f });
            var posterView = GeneratePosterView(movieDetailFrames[0]);
            movieDetailsView.AddSubview(posterView);

            var movieInfoView = GenerateMovieInfoView(movieDetailFrames[1]);
            movieDetailsView.AddSubview(movieInfoView);

            return movieDetailsView;
        }

        private UIView GeneratePosterView(CGRect frame)
        {
            var posterView = new UIView(frame);
            posterView.BackgroundColor = UIColor.White;
            _posterImageView = new UIImageView(frame.Reset().AddMargin(1.0f));
            posterView.AddSubview(_posterImageView);

            return posterView;
        }

        private async Task LoadPoster()
        {
            var poster = await _movie.PosterPath.LoadImageFromUrl();
            if (poster != null)
            {
                _posterImageView.Image = poster;
            }
            else
            {
                _posterImageView.Image = UIImage.FromBundle("Assets/Placeholder.png");
            }
        }

        private UIView GenerateMovieInfoView(CGRect frame)
        {
            var movieInfoView = new UIView(frame.AddLeftMargin(MovieExplorerAppearance.DEFAULT_MARGIN));

            //From top
            //Title label
            var titleLabel = new BoldLabel(movieInfoView.Frame.Reset(), _movie.Title);
            titleLabel.SizeToFit();
            movieInfoView.AddSubview(titleLabel);

            //Release date label
            var releaseDateLabel = new MovieExplorerLabel(movieInfoView.Frame.Reset().SetY(titleLabel.Frame.Bottom + MovieExplorerAppearance.HALF_MARGIN), _movie.ReadableReleaseDate);
            releaseDateLabel.Font = MovieExplorerAppearance.SmallFont;
            releaseDateLabel.Lines = 1;
            releaseDateLabel.SizeToFit();
            movieInfoView.AddSubview(releaseDateLabel);

            //Ratings view
            var ratingsViewOrigin = new CGPoint(0, releaseDateLabel.Frame.Bottom.AddDefaultMargin());
            var ratingsView = GenerateRatingsView(ratingsViewOrigin, _movie.VoteAverage);
            movieInfoView.AddSubview(ratingsView);

            //From bottom
            //Save favorite button
            var saveToFavoritesButtonFrame = new CGRect(0, 0, 0.0f, MovieExplorerAppearance.DEFAULT_BUTTON_HEIGHT);
            _saveToFavoritesButton = new MovieExplorerButton(saveToFavoritesButtonFrame, color: MovieExplorerAppearance.MOVIE_EXPLORER_BRIGHT_ORANGE, title: "Save to Favorites");
            _saveToFavoritesButton.Font = MovieExplorerAppearance.SmallBoldFont;
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

            //Play video button
            var playVideoButtonFrame = new CGRect(0, _saveToFavoritesButton.Frame.Top - MovieExplorerAppearance.DEFAULT_MARGIN / 2 - MovieExplorerAppearance.DEFAULT_BUTTON_HEIGHT, 1.0f, MovieExplorerAppearance.DEFAULT_BUTTON_HEIGHT);
            _playVideoButton = new MovieExplorerButton(playVideoButtonFrame, color: UIColor.Gray, title: "Loading Video...");
            _playVideoButton.Font = MovieExplorerAppearance.SmallBoldFont;
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
            if (_videos != null)
            {
                var video = _videos.FirstOrDefault(a => a.Site.ToLower().Equals("youtube"));
                if (video != null)
                {
                    UIApplication.SharedApplication.OpenUrl(NSUrl.FromString(string.Format("https://www.youtube.com/watch?v={0}", video.Key)));
                }
            }
        }

        private UIView GenerateMovieDescriptionView(CGRect frame)
        {
            var movieDescriptionView = new UIView(frame);
            frame = frame.Reset();

            var movieDescriptionLabel = new UILabel(frame.AddMargin(MovieExplorerAppearance.DEFAULT_MARGIN));
            movieDescriptionLabel.Lines = 0;
            movieDescriptionLabel.LineBreakMode = UILineBreakMode.WordWrap;
            movieDescriptionLabel.Text = _movie.Overview;
            movieDescriptionLabel.Font = MovieExplorerAppearance.SmallFont;
            movieDescriptionLabel.TextColor = UIColor.White;
            movieDescriptionLabel.SizeToFit();

            var scrollView = new UIScrollView(frame);
            scrollView.ContentInset = new UIEdgeInsets(
                top: 0.0f,
                left: 0.0f,
                bottom: MovieExplorerAppearance.DEFAULT_MARGIN * 4.0f,
                right: 0.0f);
            scrollView.ContentSize = movieDescriptionLabel.Frame.Size;
            scrollView.AddSubview(movieDescriptionLabel);

            movieDescriptionView.AddSubview(scrollView);
            return movieDescriptionView;
        }

        private HorizontalMovieScroller GenerateSimilarMoviesView(CGRect frame)
        {
            var similarMoviesViewFrame = frame.AddBottomMargin();
            var similarMoviesView = new HorizontalMovieScroller(similarMoviesViewFrame, title: "Similar Movies");
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
            if (_videos == null || _videos.Count == 0)
            {
                _playVideoButton.SetTitle("Video Unavailable", UIControlState.Normal);
                _playVideoButton.BackgroundColor = UIColor.LightGray;
            }
            else
            {
                _playVideoButton.SetTitle("Play Video", UIControlState.Normal);
                _playVideoButton.BackgroundColor = MovieExplorerAppearance.MOVIE_EXPLORER_GREEN;
            }
            _playVideoButton.SizeToFit();
            _playVideoButton.BackgroundColor = MovieExplorerAppearance.MOVIE_EXPLORER_GREEN;
        }
    }
}
