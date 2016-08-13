using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieExplorer.Core.DataAccessLayer;
using MovieExplorer.Core.ServiceLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieExplorer.Core.Tests
{
    [TestClass]
    public class LocalCacheTests
    {
        [TestMethod]
        public async Task SaveAndLoadTest()
        {
            var id = 12345;
            var title = "Test Movie I";
            var testMovie = new Movie { Id = id, Title = title };

            //Save
            await LocalCache.Instance.Save(testMovie.Id.ToString(), testMovie);
            testMovie = null;
            Assert.IsNull(testMovie);

            //Load
            testMovie = await LocalCache.Instance.Load<Movie>(id.ToString());

            Assert.IsNotNull(testMovie);
            Assert.AreEqual(id, testMovie.Id);
            Assert.AreEqual(title, testMovie.Title);
        }
    }
}
