using System;
using AppKit;
using CoreGraphics;
using Foundation;

namespace ImageDiffDemo
{
    internal class CenteredNSClipView : NSClipView
    {
        public override CoreGraphics.CGRect ConstrainBoundsRect(CGRect proposedBounds)
        {
            if (DocumentView == null)
                return base.ConstrainBoundsRect(proposedBounds);

            CGRect newClipBoundsRect = base.ConstrainBoundsRect(proposedBounds);

            NSEdgeInsets insets = ConvertetContentInsetsToProposedBoundsSize(newClipBoundsRect.Size);

            nfloat minYInset = IsFlipped ? insets.Top : insets.Bottom;
            nfloat maxYInset = IsFlipped ? insets.Bottom : insets.Top;
            nfloat minXInset = insets.Left;
            nfloat maxXInset = insets.Right;

            CGRect documentFrame = DocumentView.Frame;

            CGRect outsetDocumentFrame = new CGRect(
                documentFrame.GetMinX() - minXInset,
                documentFrame.GetMinY() - minYInset,
                documentFrame.Width + (minXInset + maxXInset),
                documentFrame.Height + (minYInset + maxYInset));

            if (newClipBoundsRect.Width > outsetDocumentFrame.Width)
            {
                newClipBoundsRect.X = outsetDocumentFrame.GetMinX() - (newClipBoundsRect.Width - outsetDocumentFrame.Width) / 2.0f;
            }
            else if (newClipBoundsRect.Width < outsetDocumentFrame.Width)
            {
                if (newClipBoundsRect.GetMaxX() > outsetDocumentFrame.GetMaxX())
                {
                    newClipBoundsRect.X = outsetDocumentFrame.GetMaxX() - newClipBoundsRect.Width;
                }
                else if (newClipBoundsRect.GetMinX() < outsetDocumentFrame.GetMinX())
                {
                    newClipBoundsRect.X = outsetDocumentFrame.GetMinX();
                }
            }

            if (newClipBoundsRect.Height > outsetDocumentFrame.Height)
            {
                newClipBoundsRect.Y = outsetDocumentFrame.GetMinY() - (newClipBoundsRect.Height - outsetDocumentFrame.Height) / 2.0f;
            }
            else if (newClipBoundsRect.Height < outsetDocumentFrame.Height)
            {
                if (newClipBoundsRect.GetMaxY() > outsetDocumentFrame.GetMaxY())
                {
                    newClipBoundsRect.Y = outsetDocumentFrame.GetMaxY() - newClipBoundsRect.Height;
                }
                else if (newClipBoundsRect.GetMinY() < outsetDocumentFrame.GetMinY())
                {
                    newClipBoundsRect.Y = outsetDocumentFrame.GetMinY();
                }
            }

            return BackingAlignedRect(newClipBoundsRect, NSAlignmentOptions.AllEdgesNearest);
        }

        NSEdgeInsets ConvertetContentInsetsToProposedBoundsSize(CGSize proposedBoundsSize)
        {
            double factor = Bounds.Width > 0 ? (proposedBoundsSize.Width / Bounds.Width) : 1.0;

            return new NSEdgeInsets(
                ContentInsets.Top * (nfloat)factor,
                ContentInsets.Left * (nfloat)factor,
                ContentInsets.Bottom * (nfloat)factor,
                ContentInsets.Right * (nfloat)factor);
        }
    }
}