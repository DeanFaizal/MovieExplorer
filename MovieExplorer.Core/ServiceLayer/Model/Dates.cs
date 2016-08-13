using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieExplorer.Core.ServiceLayer.Model
{    public class Dates
    {
        [JsonProperty(PropertyName = "maximum")]
        public string Maximum { get; set; }

        [JsonProperty(PropertyName = "minimum")]
        public string Minimum { get; set; }

    }
}
