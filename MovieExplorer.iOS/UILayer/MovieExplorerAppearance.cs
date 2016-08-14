using CoreGraphics;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace MovieExplorer.iOS.UILayer
{
    public static class MovieExplorerAppearance
    {
        //MARGINS
        public static readonly nfloat STATUS_BAR_HEIGHT = 20.0f;
        public static readonly nfloat NAVIGATION_BAR_HEIGHT = 44.0f;
        public static readonly nfloat TOTAL_TOP_BAR_HEIGHT = STATUS_BAR_HEIGHT + NAVIGATION_BAR_HEIGHT;

        public static readonly nfloat DEFAULT_MARGIN = 8.0f;
        public static readonly nfloat SMALL_MARGIN = 6.0f;
        public static readonly nfloat HALF_MARGIN = 4.0f;
        public static readonly nfloat BUTTON_INSETS = 6.0f;
        public static readonly nfloat DEFAULT_BUTTON_HEIGHT = 32.0f;

        //FONTS
        public static readonly string BOLD_FONT_NAME = "Arial-BoldMT";
        public static readonly nfloat LARGE_FONT_SIZE = 14.0f;
        public static UIFont GetBoldFont()
        {
            return UIFont.FromName(BOLD_FONT_NAME, LARGE_FONT_SIZE);
        }

        //SIZES
        public static readonly nfloat POSTER_HEIGHT_TO_WIDTH_RATIO = 1.5f;
        public static readonly nfloat POSTER_WIDTH_TO_HEIGHT_RATIO = 0.67f; // = 1/1.5f

        public static nfloat AddDefaultMargin(this nfloat value)
        {
            return value += DEFAULT_MARGIN;
        }

        //Colors
        public static readonly UIColor MOVIE_EXPLORER_ORANGE = UIColor.FromRGB(225, 155, 44);
        public static readonly UIColor MOVIE_EXPLORER_LIGHT_GRAY = UIColor.FromRGB(85, 85, 85);
        public static readonly UIColor MOVIE_EXPLORER_DARK_GRAY = UIColor.FromRGB(50, 50, 50);
    }
}
