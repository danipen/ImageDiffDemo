using System;
using AppKit;
using CoreGraphics;

namespace ImageDiffDemo
{
    internal static class ImageDiffExtensions
    {
        internal static float CalculateZoomToFit(
            nfloat width, nfloat height, nfloat targetWidth, nfloat targetHeight)
        {
            if (width == 0 || height == 0)
                return 1;

            float widthScale = (float)targetWidth / (float)width;
            float heightScale = (float)targetHeight / (float)height;

            return Math.Min(widthScale, heightScale);
        }

        internal static bool IsImageBiggerThanFrame(CGSize frameSize, CGSize imageSize)
        {
            return imageSize.Width > frameSize.Width ||
                   imageSize.Height > frameSize.Height;
        }

        internal static CGSize GetComposedImageSize(NSImage leftImage, NSImage rightImage)
        {
            if (leftImage == null || rightImage == null)
                return CGSize.Empty;

            return new CGSize(
                Math.Max(leftImage.Size.Width, rightImage.Size.Width),
                Math.Max(leftImage.Size.Height, rightImage.Size.Height));
        }

        internal static CGRect CalculateCenteredRectangle(CGRect frame, NSImage image, float zoomLevel)
        {
            nfloat x = (frame.Width - image.Size.Width * zoomLevel) / 2;
            nfloat y = (frame.Height - image.Size.Height * zoomLevel) / 2;

            return new CGRect(x, y, image.Size.Width * zoomLevel, image.Size.Height * zoomLevel);
        }
    }
}