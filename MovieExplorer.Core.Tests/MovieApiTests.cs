using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieExplorer.Core.ServiceLayer.Model;
using System.Collections.Generic;
using MovieExplorer.Core.ServiceAccessLayer;
using System.Threading.Tasks;
using System.Linq;

namespace MovieExplorer.Core.Tests
{
    [TestClass]
    public class MovieApiTests
    {
        [TestMethod]
        public async Task TestNowPlaying()
        {
            var results = await MovieAccessor.Instance.GetNowPlaying();
            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results, typeof(List<Movie>));
            Assert.AreNotEqual(0, results.Count);
        }

        [TestMethod]
        public async Task TestTopRated()
        {
            var results = await MovieAccessor.Instance.GetNowPlaying();
            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results, typeof(List<Movie>));
            Assert.AreNotEqual(0, results.Count);
        }

        [TestMethod]
        public async Task TestPopular()
        {
            var results = await MovieAccessor.Instance.GetNowPlaying();
            Assert.IsNotNull(results);
            Assert.IsInstanceOfType(results, typeof(List<Movie>));
            Assert.AreNotEqual(0, results.Count);
        }
        
        [TestMethod]
        public async Task TestGetSimilar()
        {
            var nowPlaying = await MovieAccessor.Instance.GetNowPlaying();
            var movie = nowPlaying.FirstOrDefault();
            Assert.IsNotNull(movie);
            if (movie!=null)
            {
                var similarMovies = await MovieAccessor.Instance.GetSimilar(movie.Id);
                Assert.IsNotNull(similarMovies);
                Assert.AreNotEqual(0, similarMovies.Count);
                Assert.IsInstanceOfType(similarMovies, typeof(List<Movie>));
            }
        }

        [TestMethod]
        public async Task TestGetVideos()
        {

            var movieResults = await MovieAccessor.Instance.GetNowPlaying();
            var movie = movieResults.FirstOrDefault();
            Assert.IsNotNull(movie);
            if (movie != null)
            {
                var videos = await MovieAccessor.Instance.GetVideos(movie.Id);
                Assert.IsNotNull(videos);
                Assert.AreNotEqual(0, videos.Count);
                Assert.IsInstanceOfType(videos, typeof(List<Video>));
            }
        }

    }
}
