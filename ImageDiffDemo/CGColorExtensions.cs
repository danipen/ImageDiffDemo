using System;
using CoreGraphics;

namespace ImageDiffDemo
{
    internal static class CGColorExtensions
    {
        internal static CGColor CreateCGColor(int alpha, int r, int g, int b)
        {
            return new CGColor(r / 255.0f, g / 255.0f, b / 255.0f, alpha / 255.0f);
        }
    }
}
