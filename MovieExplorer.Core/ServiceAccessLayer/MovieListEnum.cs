using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieExplorer.Core.BusinessLayer;

namespace MovieExplorer.Core.ServiceAccessLayer
{
    public enum MovieListType
    {
        [EnumDescription("Top Rated")]
        TopRated,
        [EnumDescription("Popular")]
        Popular,
        [EnumDescription("Now Playing")]
        NowPlaying,
        [EnumDescription("Similar")]
        Similar,
        [EnumDescription("Search Results")]
        SearchResults        
    }
}
