using CoreGraphics;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace MovieExplorer.iOS.UILayer.Controls
{
    public class BoldLabel : UILabel
    {
        public BoldLabel(CGRect frame, string text) : base(frame)
        {
            Text = text;
            Font = MovieExplorerAppearance.GetBoldFont();
            TextColor = UIColor.White;
        }
    }
}
