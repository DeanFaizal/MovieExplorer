﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieExplorer.Core.ServiceLayer.Model
{
    public class Movie
    {
        private string _posterPath;
        [JsonProperty(PropertyName = "poster_path")]
        public string PosterPath
        {
            get
            {
                return _posterPath;
            }
            set
            {
                _posterPath = value;
                _posterPath = string.Format("http://image.tmdb.org/t/p/w500/{0}", _posterPath);
            }
        }

        [JsonProperty(PropertyName = "adult")]
        public bool Adult { get; set; }

        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

        [JsonProperty(PropertyName = "release_date")]
        public string ReleaseDate { get; set; }

        [JsonProperty(PropertyName = "genre_ids")]
        public List<object> GenreIds { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "original_title")]
        public string OriginalTitle { get; set; }

        [JsonProperty(PropertyName = "original_language")]
        public string OriginalLanguage { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "backdrop_path")]
        public string BackdropPath { get; set; }

        [JsonProperty(PropertyName = "popularity")]
        public double Popularity { get; set; }

        [JsonProperty(PropertyName = "vote_count")]
        public int VoteCount { get; set; }

        [JsonProperty(PropertyName = "video")]
        public bool IsVideoAvailable { get; set; }

        [JsonProperty(PropertyName = "vote_average")]
        public double VoteAverage { get; set; }

        public string ReadableReleaseDate
        {
            get
            {
                DateTime releaseDateTime;
                if (string.IsNullOrEmpty(ReleaseDate) ||
                    !DateTime.TryParse(ReleaseDate, out releaseDateTime))
                {
                    return "Release date unavailable";
                }
                else
                {
                    return string.Format("Release Date: {0}", releaseDateTime.ToString("M/d/yyyy"));
                }
            }
        }
    }
}
