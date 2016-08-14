using CoreGraphics;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace MovieExplorer.iOS.UILayer
{
    public static class FrameExtensions
    {
        public static CGRect[] DivideHorizontal(this CGRect frame, int horizontalSections)
        {
            var frames = new CGRect[horizontalSections];
            var sectionWidth = frame.Width / horizontalSections;

            for (int i = 0; i < horizontalSections; i++)
            {
                frames[i] = new CGRect((sectionWidth * i) + frame.X, frame.Y, sectionWidth, frame.Height);
            }

            return frames;
        }

        public static CGRect[] DivideHorizontal(this CGRect frame, params float[] weights)
        {
            if (weights == null || weights.Length == 0)
            {
                return new CGRect[1] { frame };
            }

            var sections = weights.Length;
            var weightTotal = weights.Sum();

            var frames = new CGRect[weights.Length];
            var currentX = frame.X;
            for (int i = 0; i < weights.Length; i++)
            {
                var sectionFrameWidth = (weights[i] / weightTotal) * frame.Width;
                frames[i] = new CGRect(currentX, frame.Y, sectionFrameWidth, frame.Height);
                currentX += sectionFrameWidth;
            }
            return frames;
        }

        public static CGRect[] DivideVertical(this CGRect frame, int verticalSections)
        {
            var frames = new CGRect[verticalSections];
            var sectionHeight = frame.Height / verticalSections;

            for (int i = 0; i < verticalSections; i++)
            {
                frames[i] = new CGRect(frame.X, (sectionHeight * i) + frame.Y, frame.Width, sectionHeight);
            }

            return frames;
        }

        public static CGRect Reset(this CGRect frame)
        {
            return new CGRect(0, 0, frame.Width, frame.Height);
        }

        public static CGRect AddTopMargin(this CGRect frame, nfloat margin)
        {
            return new CGRect(frame.X, frame.Y + margin, frame.Width, frame.Height - margin);
        }
        public static CGRect AddTopMargin(this CGRect frame)
        {
            return AddTopMargin(frame, MovieExplorerAppearance.DEFAULT_MARGIN);
        }

        public static CGRect AddBottomMargin(this CGRect frame, nfloat margin)
        {
            return new CGRect(frame.X, frame.Y, frame.Width, frame.Height - margin);
        }
        public static CGRect AddBottomMargin(this CGRect frame)
        {
            return AddBottomMargin(frame, MovieExplorerAppearance.DEFAULT_MARGIN);
        }

        public static CGRect AddLeftMargin(this CGRect frame, nfloat margin)
        {
            return new CGRect(frame.X + margin, frame.Y, frame.Width - margin, frame.Height);
        }
        public static CGRect AddLeftMargin(this CGRect frame)
        {
            return AddLeftMargin(frame, MovieExplorerAppearance.DEFAULT_MARGIN);
        }

        public static CGRect AddRightMargin(this CGRect frame, nfloat margin)
        {
            return new CGRect(frame.X, frame.Y, frame.Width - margin, frame.Height);
        }
        public static CGRect AddRightMargin(this CGRect frame)
        {
            return AddRightMargin(frame, MovieExplorerAppearance.DEFAULT_MARGIN);
        }


        //Shrinks frame by margin
        public static CGRect AddMargin(this CGRect frame, nfloat margin)
        {
            return new CGRect(frame.X + margin, frame.Y + margin, frame.Width - (2 * margin), frame.Height - (2 * margin));
        }

        public static CGRect AddDefaultMargin(this CGRect frame)
        {
            return frame.AddMargin(MovieExplorerAppearance.DEFAULT_MARGIN);
        }

        //Expands frame by margin
        public static CGRect Expand(this CGRect frame, nfloat margin)
        {
            return frame.AddMargin(-margin);
        }

        public static CGPoint GetCenter(this CGRect frame)
        {
            return new CGPoint(frame.Width / 2, frame.Height / 2);
        }

        public static CGRect SetX(this CGRect frame, nfloat x)
        {
            return new CGRect(x, frame.Y, frame.Width, frame.Height);
        }

        public static CGRect SetY(this CGRect frame, nfloat y)
        {
            return new CGRect(frame.X, y, frame.Width, frame.Height);
        }

        public static CGRect SetSize(this CGRect frame, CGPoint size)
        {
            return new CGRect(frame.X, frame.Y, size.X, size.Y);
        }
    }
}
