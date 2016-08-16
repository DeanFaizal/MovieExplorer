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

        public static nfloat DEFAULT_MARGIN = 8.0f;
        public static readonly nfloat SMALL_MARGIN = 6.0f;
        public static readonly nfloat HALF_MARGIN = 4.0f;
        public static readonly nfloat BUTTON_INSETS = 6.0f;
        public static readonly nfloat DEFAULT_BUTTON_HEIGHT = 32.0f;


        //SIZES
        public static readonly nfloat POSTER_HEIGHT_TO_WIDTH_RATIO = 1.5f;
        public static readonly nfloat POSTER_WIDTH_TO_HEIGHT_RATIO = 0.67f; // = 1/1.5f

        public static nfloat AddDefaultMargin(this nfloat value)
        {
            return value += DEFAULT_MARGIN;
        }


        //Colors
        public static readonly UIColor MOVIE_EXPLORER_ORANGE = UIColor.FromRGB(225, 155, 44);
        public static readonly UIColor MOVIE_EXPLORER_GREEN = UIColor.FromRGB(163, 202, 25);
        public static readonly UIColor MOVIE_EXPLORER_BRIGHT_ORANGE = UIColor.FromRGB(219, 176, 1);
        public static readonly UIColor MOVIE_EXPLORER_EXTRA_LIGHT_GRAY = UIColor.FromRGB(120, 120, 120);
        public static readonly UIColor MOVIE_EXPLORER_LIGHT_GRAY = UIColor.FromRGB(85, 85, 85);
        public static readonly UIColor MOVIE_EXPLORER_DARK_GRAY = UIColor.FromRGB(50, 50, 50);


        //FONTS
        public static readonly string STANDARD_FONT_NAME = "ArialMT";
        public static readonly string BOLD_FONT_NAME = "Arial-BoldMT";
        public static readonly nfloat NAV_TITLE_FONT_SIZE = 16.0f;
        public static readonly nfloat STANDARD_FONT_SIZE = 14.0f;
        public static readonly nfloat SMALL_FONT_SIZE = 12.0f;
        public static readonly nfloat MICRO_FONT_SIZE = 10.0f;

        public static UIFont NavBarTitleFont
        {
            get
            {
                return UIFont.FromName(BOLD_FONT_NAME, NAV_TITLE_FONT_SIZE);
            }
        }

        public static UIFont BoldFont
        {
            get
            {
                return UIFont.FromName(BOLD_FONT_NAME, STANDARD_FONT_SIZE);
            }
        }

        public static UIFont SmallBoldFont
        {
            get
            {
                return UIFont.FromName(BOLD_FONT_NAME, SMALL_FONT_SIZE);
            }
        }

        public static UIFont StandardFont
        {
            get
            {
                return UIFont.FromName(STANDARD_FONT_NAME, STANDARD_FONT_SIZE);
            }
        }

        public static UIFont SmallFont
        {
            get
            {
                return UIFont.FromName(STANDARD_FONT_NAME, SMALL_FONT_SIZE);
            }
        }

        public static UIFont MicroFont
        {
            get
            {
                return UIFont.FromName(STANDARD_FONT_NAME, MICRO_FONT_SIZE);
            }
        }


        //DEVICE
        public static bool IsShortDevice //Short iPhone (iPhone 4s & earlier) or iPad devices
        {
            get
            {
                var tallDeviceHeightToWidthRatio = 1.775f; //from iPhone 6s
                var screenBounds = UIScreen.MainScreen.Bounds;
                var screenHeightToWidthRatio = screenBounds.Height / screenBounds.Width;
                if (screenHeightToWidthRatio >= tallDeviceHeightToWidthRatio)
                {
                    return false;
                }
                return true;
            }
        }
    }
}
