using System;

using Foundation;
using AppKit;
using CoreGraphics;
using ImageDiffDemo.Common;

namespace ImageDiffDemo
{
    public partial class MainWindow : NSWindow, IImageDiffView, IProgressControls
    {
        public MainWindow() : base()
        {
            Title = "Image viewer";

            StyleMask = NSWindowStyle.Closable | NSWindowStyle.Miniaturizable |
                NSWindowStyle.Resizable | NSWindowStyle.Titled;

            SetFrame(new CGRect(0, 0, 1124, 868), true);

            AutorecalculatesKeyViewLoop = true;

            ContentView = BuildComponents();
        }

        unsafe void BrowseButton_Activated(object sender, EventArgs e)
        {
            NSOpenPanel panel = new NSOpenPanel();
            panel.CanChooseFiles = true;
            panel.CanChooseDirectories = false;
            panel.AllowsMultipleSelection = true;
            panel.CanCreateDirectories = true;

            using (panel)
            {
                nint result = panel.RunModal();
                if (result != 1)
                    return;

                NSUrl[] urls = panel.Urls;
                if (urls.Length == 0)
                    return;

                NSUrl leftImagePath = urls[1];
                NSUrl rightImagePath = urls[0];

                mLeftImage = new NSImage(leftImagePath.Path, false);
                mRightImage = new NSImage(rightImagePath.Path, false);

                NSBitmapImageRep leftImageRep =
                    ImageDiffUtils.GetBitmapRepresentation(mLeftImage);
                
                NSBitmapImageRep rightImageRep =
                    ImageDiffUtils.GetBitmapRepresentation(mRightImage);

                nint leftLen = leftImageRep.BytesPerRow * leftImageRep.PixelsHigh;
                nint rightLen = rightImageRep.BytesPerRow * rightImageRep.PixelsHigh;

                byte* leftBitmapData = (byte*)leftImageRep.BitmapData;
                byte* rightBitmapData = (byte*)rightImageRep.BitmapData;

                ImageDiffOperations.CalculateImageDiff(
                    this, this, leftBitmapData, rightBitmapData,
                    (int)leftLen, (int)rightLen);
            }
        }

        void OneToOneButton_Activated(object sender, EventArgs e)
        {
            mImageDiffView.ZoomOneToOne();
        }

        void FitButton_Activated(object sender, EventArgs e)
        {
            mImageDiffView.ZoomToFit();
        }

        void ZoomOutButton_Activated(object sender, EventArgs e)
        {
            mImageDiffView.ZoomOut();
        }

        void ZoomInButton_Activated(object sender, EventArgs e)
        {
            mImageDiffView.ZoomIn();
        }

        void IImageDiffView.SetDiffImageBytes(byte[] diffImage)
        {
            mImageDiffView.SetImage(
                ImageDiffUtils.GetDiffImage(mLeftImage, mRightImage, diffImage));
        }

        void IProgressControls.ShowProgress()
        {
            mSpinner.Hidden = false;
            mSpinner.StartAnimation(this);
        }

        void IProgressControls.HideProgress()
        {
            mSpinner.StopAnimation(this);
            mSpinner.Hidden = true;
        }

        void IProgressControls.ShowInformation(string message)
        {
            var alert = new NSAlert();
            alert.MessageText = "ImageDiffDemo";
            alert.InformativeText = message;
            alert.AlertStyle = NSAlertStyle.Informational;
            alert.BeginSheet(this);
        }

        void IProgressControls.ShowError(string message)
        {
            var alert = new NSAlert();
            alert.MessageText = "ImageDiffDemo";
            alert.InformativeText = message;
            alert.AlertStyle = NSAlertStyle.Critical;
            alert.BeginSheet(this);
        }

        NSView BuildComponents()
        {
            NSView result = new NSView();

            NSButton browseButton = new NSButton();
            browseButton.Title = "Browse...";
            browseButton.BezelStyle = NSBezelStyle.Rounded;
            browseButton.Activated += BrowseButton_Activated;

            NSButton oneToOneButton = new NSButton();
            oneToOneButton.Title = "Zoom 1 to 1";
            oneToOneButton.BezelStyle = NSBezelStyle.Rounded;
            oneToOneButton.Activated += OneToOneButton_Activated;

            NSButton fitButton = new NSButton();
            fitButton.Title = "Zoom to fit";
            fitButton.BezelStyle = NSBezelStyle.Rounded;
            fitButton.Activated += FitButton_Activated;

            NSButton zoomOutButton = new NSButton();
            zoomOutButton.Title = "Zoom out";
            zoomOutButton.Image = NSImage.ImageNamed(NSImageName.RemoveTemplate);
            zoomOutButton.BezelStyle = NSBezelStyle.Rounded;
            zoomOutButton.Activated += ZoomOutButton_Activated;

            NSButton zoomInButton = new NSButton();
            zoomInButton.Title = "Zoom in";
            zoomInButton.Image = NSImage.ImageNamed(NSImageName.AddTemplate);
            zoomInButton.BezelStyle = NSBezelStyle.Rounded;
            zoomInButton.Activated += ZoomInButton_Activated;

            mImageDiffView = new ImageContentView();

            mSpinner = new NSProgressIndicator();
            mSpinner.Style = NSProgressIndicatorStyle.Spinning;
            mSpinner.ControlSize = NSControlSize.Small;
            mSpinner.Hidden = true;

            ControlPacker.AddControls(
                result, new string[]{
                "H:|-20-[browseButton]-5-[oneToOneButton]-5-[fitButton]-5-[zoomOutButton]-5-[zoomInButton]-8-[spinner(16)]",
                "H:|-20-[imageView]-20-|",
                "V:|-20-[browseButton]-10-[imageView]-20-|",
                "V:|-20-[oneToOneButton]",
                "V:|-20-[fitButton]",
                "V:|-20-[zoomOutButton]",
                "V:|-20-[zoomInButton]",
                "V:|-23-[spinner(16)]"
            },
                new NSDictionary(
                    "browseButton", browseButton,
                    "oneToOneButton", oneToOneButton,
                    "fitButton", fitButton,
                    "zoomOutButton", zoomOutButton,
                    "zoomInButton", zoomInButton,
                    "imageView", mImageDiffView,
                    "spinner", mSpinner));

            return result;
        }

        ImageContentView mImageDiffView;

        NSProgressIndicator mSpinner;

        NSImage mLeftImage;
        NSImage mRightImage;
    }
}

