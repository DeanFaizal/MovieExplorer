using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace MovieExplorer.iOS.UILayer.ViewControllers
{
    public class BaseViewController : UIViewController
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            InitializeNavBar();
        }

        private void InitializeNavBar()
        {
            NavigationController.NavigationBar.BarTintColor = MovieExplorerAppearance.MOVIE_EXPLORER_DARK_GRAY;
            NavigationController.NavigationBar.TintColor = MovieExplorerAppearance.MOVIE_EXPLORER_ORANGE;
            NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes
            {
                Font = MovieExplorerAppearance.NavBarTitleFont,
                ForegroundColor = MovieExplorerAppearance.MOVIE_EXPLORER_ORANGE
            };
            NavigationController.NavigationBar.BarStyle = UIBarStyle.Black;
        }
    }
}
