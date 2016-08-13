using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieExplorer.Core.ServiceLayer.Model
{
    public class MovieResults
    {
        [JsonProperty(PropertyName = "page")]
        public int Page { get; set; }

        [JsonProperty(PropertyName = "results")]
        public List<Movie> Movies { get; set; }

        [JsonProperty(PropertyName = "dates")]
        public Dates Dates { get; set; }

        [JsonProperty(PropertyName = "total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty(PropertyName = "total_results")]
        public int TotalResults { get; set; }
    }
}
