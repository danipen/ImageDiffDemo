using System;
using CoreGraphics;

namespace ImageDiffDemo
{
    internal static class ImageDiffColors
    {
        internal static CGColor LeftImageBorderColor =
            CGColorExtensions.CreateCGColor(255, 242, 77, 77);
        internal static CGColor RightImageBorderColor =
            CGColorExtensions.CreateCGColor(255, 87, 184, 73);
        internal static CGColor TransparentBackgroundSquareColor =
            CGColorExtensions.CreateCGColor(255, 240, 240, 240);
        internal static CGColor ImageBorderColor =
                CGColorExtensions.CreateCGColor(255, 232, 232, 232);

        internal class Swipe
        {
            internal static CGColor SwipeLineColor =
                CGColorExtensions.CreateCGColor(255, 255, 0, 0);
            internal static CGColor KnobtBorderColor =
                CGColorExtensions.CreateCGColor(255, 174, 174, 174);
            internal static CGColor KnobtClickedBorderColor =
                CGColorExtensions.CreateCGColor(255, 63, 149, 252);
            internal static CGColor KnobtBackgroundColor =
                CGColorExtensions.CreateCGColor(255, 245, 245, 245);
            internal static CGColor KnobtHoveredBackgroundColor =
                CGColorExtensions.CreateCGColor(255, 235, 235, 235);
        }

        internal class Blended
        {
            internal static CGColor SliderBackgroundColor =
                CGColorExtensions.CreateCGColor(255, 250, 250, 250);
            internal static CGColor SliderBorderColor =
                CGColorExtensions.CreateCGColor(255, 232, 232, 232);
        }
    }
}