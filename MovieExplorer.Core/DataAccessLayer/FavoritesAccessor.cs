using MovieExplorer.Core.ServiceLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieExplorer.Core.DataAccessLayer
{
    /// <summary>
    /// Saves favorite movie IDs
    /// </summary>
    public class FavoritesAccessor
    {
        private static Lazy<FavoritesAccessor> _instance = new Lazy<FavoritesAccessor>();

        public static FavoritesAccessor Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private readonly string FAVORITES_LIST_KEY = "FavoritesListKey";

        public async Task<List<string>> GetFavorites()
        {
            var _favorites = new List<string>();
            try
            {
                _favorites = await LocalCache.Instance.Load<List<string>>(FAVORITES_LIST_KEY);
            }
            catch (Exception ex)
            {
                await LocalCache.Instance.Save(FAVORITES_LIST_KEY, _favorites); //save the empty favorites list since it doesn't exist
            }
            return _favorites;
        }

        public async Task SaveFavorites(List<string> favorites)
        {
            await LocalCache.Instance.Save(FAVORITES_LIST_KEY, favorites);
        }

        public async Task AddToFavorites(string movieId)
        {
            var favorites = await GetFavorites();
            if (!favorites.Contains(movieId))
            {
                favorites.Add(movieId);
                await SaveFavorites(favorites);
            }
        }

        public async Task RemoveFromFavorites(string movieId)
        {
            var favorites = await GetFavorites();
            var movieToRemove = favorites.FirstOrDefault(a => a.Equals(movieId));
            if (movieToRemove != null)
            {
                favorites.Remove(movieToRemove);
                await SaveFavorites(favorites);
            }
        }

        public async Task<bool> IsFavorite(string movieId)
        {
            var favorites = await GetFavorites();
            if (favorites.Any(a => a.Equals(movieId)))
            {
                return true;
            }
            return false;
        }
    }
}
