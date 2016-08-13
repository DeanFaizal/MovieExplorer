using CoreGraphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieExplorer.iOS.UILayer
{
    public static class MovieExplorerAppearance
    {
        public static readonly nfloat STATUS_BAR_HEIGHT = 20.0f;
        public static readonly nfloat NAVIGATION_BAR_HEIGHT = 44.0f;
        public static readonly nfloat TOTAL_TOP_BAR_HEIGHT = STATUS_BAR_HEIGHT + NAVIGATION_BAR_HEIGHT;

        public static readonly nfloat DEFAULT_MARGIN = 8.0f;
        public static readonly nfloat SMALL_MARGIN = 6.0f;
        public static readonly nfloat BUTTON_INSETS = 6.0f;
        public static readonly nfloat DEFAULT_BUTTON_HEIGHT = 32.0f;

        public static readonly nfloat LARGE_FONT_SIZE = 14.0f;

        public static readonly CGSize POSTER_SIZE = new CGSize(125, 187.5);

        public static readonly nfloat POSTER_HEIGHT_TO_WIDTH_RATIO = 1.5f;

        public static nfloat AddDefaultMargin(this nfloat value)
        {
            return value += DEFAULT_MARGIN;
        }
    }
}
