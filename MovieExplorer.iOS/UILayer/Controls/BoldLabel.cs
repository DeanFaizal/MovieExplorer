﻿using CoreGraphics;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace MovieExplorer.iOS.UILayer.Controls
{
    public class BoldLabel : UILabel
    {
        public BoldLabel(CGRect frame, string text) : this(frame, text, UIColor.White) { }

        public BoldLabel(CGRect frame, string text, UIColor textColor) : base(frame)
        {
            Lines = 0;
            Text = text;
            Font = MovieExplorerAppearance.BoldFont;
            TextColor = textColor;
            LineBreakMode = UILineBreakMode.WordWrap;
        }
    }
}
