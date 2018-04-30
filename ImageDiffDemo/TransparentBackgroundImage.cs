using System;
using AppKit;
using CoreGraphics;

namespace ImageDiffDemo
{
    internal static class TransparentBackgroundImage
    {
        internal static NSBitmapImageRep Create()
        {
            float squareSize = 6;

            NSBitmapImageRep image = new NSBitmapImageRep(new CGRect(0, 0, squareSize * 2, squareSize * 2));

            using (CGContext gc = NSGraphicsContext.FromBitmap(image).GraphicsPort)
            {
                gc.SetFillColor(ImageDiffColors.TransparentBackgroundSquareColor);
                gc.FillRect(new CGRect(0, 0, squareSize, squareSize));
                gc.FillRect(new CGRect(squareSize, squareSize, squareSize, squareSize));
            }

            return image;
        }
    }
}
