using System;
using System.IO;
using System.Linq;
using MovieExplorer.Core.ServiceLayer.Model;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.iOS;
using Xamarin.UITest.Queries;

namespace iOSUITest
{
	[TestFixture]
	public class Tests
	{
		iOSApp app;

		[SetUp]
		public void BeforeEachTest ()
		{
			// TODO: If the iOS app being tested is included in the solution then open
			// the Unit Tests window, right click Test Apps, select Add App Project
			// and select the app projects that should be tested.
			app = ConfigureApp
				.iOS
				.EnableLocalScreenshots ()
				// TODO: Update this path to point to your iOS app and uncomment the
				// code if the app is not included in the solution.
				.AppBundle ("../../../MovieExplorer.iOS/bin/iPhoneSimulator/Debug/MovieExploreriOS.app")
				.StartApp ();
		}

		[Test]
		public void AppLaunches ()
		{
			app.Screenshot ("Main screen");
		}

		[Test]
		public void OpenDetails ()
		{
			app.Tap (a => a.Class ("MovieExplorer_iOS_UILayer_Controls_MovieCell"));
			app.Screenshot ("Details screen");
		}

		[Test]
		public void SaveToFavorites ()
		{
			app.Tap (a => a.Class ("MovieExplorer_iOS_UILayer_Controls_MovieCell"));
			app.Tap (a => a.Marked ("Save to Favorites"));
			app.Screenshot ("Save to Favorites");
		}

		[Test]
		public void PlayVideo ()
		{
			app.Tap (a => a.Class ("MovieExplorer_iOS_UILayer_Controls_MovieCell"));
			app.Tap (a => a.Marked ("Play Video"));
			app.Screenshot ("Playing video");
		}

		[Test]
		public void OpenSimilarMovieDetails ()
		{
			app.Tap (a => a.Class ("MovieExplorer_iOS_UILayer_Controls_MovieCell"));
			app.Tap (a => a.Class ("MovieExplorer_iOS_UILayer_Controls_MovieCell"));
			app.Screenshot ("Similar movies details screen");
		}

		[Test]
		public void SearchForMovie ()
		{
			app.Tap (a => a.Marked ("Search"));
			app.EnterText (a => a.Class ("UISearchBar"), "Interstellar");
			app.Screenshot ("Searched for Interstellar");
		}
	}
}

