using System;
using AppKit;
using CoreGraphics;

namespace ImageDiffDemo
{
    internal class ImageView : NSView
    {
        internal void SetDrawBackground(bool value)
        {
            mDrawBackground = value;

            NeedsDisplay = true;
        }

        internal void SetImage(NSImage image)
        {
            mImage = image;

            NeedsDisplay = true;
        }

        public override void DrawRect(CoreGraphics.CGRect dirtyRect)
        {
            if (mImage == null)
            {
                base.DrawRect(dirtyRect);
                return;
            }

            using (CGContext gc = NSGraphicsContext.CurrentContext.GraphicsPort)
            {
                if (mDrawBackground)
                    DrawBackground(gc);

                gc.DrawImage(Frame, mImage.CGImage);
                DrawImageBorder(gc, Frame, ImageDiffColors.ImageBorderColor);
            }

        }

        void DrawBackground(CGContext gc)
        {
            CGImage bgImage = GetBackgroundBitmap().CGImage;
            gc.DrawTiledImage(new CGRect(0, 0, bgImage.Width, bgImage.Height), bgImage);
        }

        void DrawImageBorder(CGContext gc, CGRect rectangle, CGColor strokeColor)
        {
            gc.SetLineWidth(1);
            gc.SetStrokeColor(strokeColor);
            rectangle.Inflate(-0.5, -0.5);
            gc.StrokeRect(rectangle);
        }

        NSBitmapImageRep GetBackgroundBitmap()
        {
            if (mBackgroundBitmap == null)
                mBackgroundBitmap = TransparentBackgroundImage.Create();

            return mBackgroundBitmap;
        }

        NSImage mImage;
        bool mDrawBackground;
        static NSBitmapImageRep mBackgroundBitmap;
    }
}