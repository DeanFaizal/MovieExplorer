using MovieExplorer.Core.ServiceLayer;
using MovieExplorer.Core.ServiceLayer.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MovieExplorer.Core.ServiceAccessLayer
{
    public class MovieAccessor
    {
        private static Lazy<MovieAccessor> _instance = new Lazy<MovieAccessor>();

        public static MovieAccessor Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private readonly string API_KEY = "ab41356b33d100ec61e6c098ecc92140";
        private readonly string ROOT_URL = "http://api.themoviedb.org/3/movie/";


        private async Task<T> Fetch<T>(string url)
        {
            try
            {
                var rawResult = string.Empty;
                using (var client = new HttpClient())
                {
                    rawResult = await client.GetStringAsync(url);
                }
                var results = JsonConvert.DeserializeObject<T>(rawResult);
                return results;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        private async Task<T> FetchMovieList<T>(string endpoint, int page = 1)
        {
            var url = string.Format("{0}{1}?api_key={2}&sort_by=popularity.des&page={3}", ROOT_URL, endpoint, API_KEY, page);
            return await Fetch<T>(url);
        }
        private async Task<T> FetchForMovie<T>(string endpoint, int movieId)
        {
            var url = string.Format("{0}{1}/{2}?api_key={3}", ROOT_URL, movieId, endpoint, API_KEY);
            return await Fetch<T>(url);
        }


        public async Task<List<Movie>> GetNowPlaying(int page = 1)
        {
            try
            {
                var results = await FetchMovieList<MovieResults>(endpoint: "now_playing", page: page);
                return results.Movies;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<List<Movie>> GetTopRated(int page = 1)
        {
            var results = await FetchMovieList<MovieResults>(endpoint: "top_rated", page: page);
            return results.Movies;
        }

        public async Task<List<Movie>> GetPopular(int page = 1)
        {
            var results = await FetchMovieList<MovieResults>(endpoint: "popular", page: page);
            return results.Movies;
        }


        public async Task<List<Movie>> GetSimilar(int movieId)
        {
            var results = await FetchForMovie<MovieResults>(endpoint: "similar", movieId: movieId);
            return results.Movies;
        }

        public async Task<List<Video>> GetVideos(int movieId)
        {
            var results = await FetchForMovie<VideoResults>(endpoint: "videos", movieId: movieId);
            return results.Videos;
        }

    }
}
