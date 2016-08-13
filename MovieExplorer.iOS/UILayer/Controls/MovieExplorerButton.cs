using CoreGraphics;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace MovieExplorer.iOS.UILayer.Controls
{
    public class MovieExplorerButton : UIButton
    {
        public MovieExplorerButton(CGRect frame, UIColor color, string title) : base(frame)
        {
            ContentEdgeInsets = new UIEdgeInsets(MovieExplorerAppearance.BUTTON_INSETS,
                MovieExplorerAppearance.BUTTON_INSETS,
                MovieExplorerAppearance.BUTTON_INSETS,
                MovieExplorerAppearance.BUTTON_INSETS);

            BackgroundColor = color;
            SetTitleColor(UIColor.White, UIControlState.Normal);

            SetTitle(title, UIControlState.Normal);
            Font = UIFont.BoldSystemFontOfSize(MovieExplorerAppearance.LARGE_FONT_SIZE);
        }
    }
}
