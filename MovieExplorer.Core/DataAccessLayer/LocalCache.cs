using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using Akavache;

namespace MovieExplorer.Core.DataAccessLayer
{
   public class LocalCache
    {
        private static bool _isInitialized = false;
        private static Lazy<LocalCache> _instance = new Lazy<LocalCache>();

        public static LocalCache Instance
        {
            get {
                if (!_isInitialized)
                {
                    BlobCache.ApplicationName = "MovieExplorer";
                    _isInitialized = true;
                }
                return _instance.Value; }
        }

        public async Task Save<T>(string id, T item)
        {
            await BlobCache.UserAccount.InsertObject(id, item);
        }

        public async Task<T> Load<T>(string id)
        {
            return await BlobCache.UserAccount.GetObject<T>(id);
        }
    }
}
