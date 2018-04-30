using System;
using System.Runtime.InteropServices;
using AppKit;
using CoreGraphics;

namespace ImageDiffDemo
{
    public class ImageDiffUtils
    {
        internal static NSBitmapImageRep GetBitmapRepresentation(NSImage image)
        {
            foreach (NSImageRep representation in image.Representations())
            {
                if (representation is NSBitmapImageRep)
                    return (NSBitmapImageRep)representation;
            }

            return null;
        }

        internal static NSImage GetDiffImage(
            NSImage leftImageRep,
            NSImage rightImageRep,
            byte[] outputBytes)
        {
            NSBitmapImageRep output = new NSBitmapImageRep(leftImageRep.CGImage);

            CGRect outputRect = new CGRect(0, 0,
                Math.Max(leftImageRep.CGImage.Width, rightImageRep.CGImage.Width),
                Math.Max(leftImageRep.CGImage.Height, rightImageRep.CGImage.Height));

            Marshal.Copy(outputBytes, 0, output.BitmapData, (int)outputBytes.Length);

            return new NSImage(output.CGImage, outputRect.Size);
        }
    }
}
