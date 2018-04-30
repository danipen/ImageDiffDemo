using System;
using AppKit;
using CoreAnimation;
using CoreGraphics;
using Foundation;

namespace ImageDiffDemo
{
    public class ImageContentView : NSScrollView
    {
        internal ImageContentView(IntPtr intPtr) : base(intPtr) { }

        internal ImageContentView()
        {
            ContentView = new CenteredNSClipView();

            BuildComponents();
        }

        internal void SetImage(NSImage image)
        {
            mImage = image;

            InitZoom();

            mImageView.SetImage(image);
        }

        internal void ZoomOneToOne()
        {
            AnimateZoomLevel(1);
        }

        internal void ZoomToFit()
        {
            AnimateZoomLevel(GetZoomValueToFit());
        }

        internal void ZoomOut()
        {
            AnimateZoomLevel(mZoomLevel - ZOOM_STEP);
        }

        internal void ZoomIn()
        {
            AnimateZoomLevel(mZoomLevel + ZOOM_STEP);
        }

        void InitZoom()
        {
            if (ImageDiffExtensions.IsImageBiggerThanFrame(Frame.Size, mImage.Size))
            {
                ZoomLevel = GetZoomValueToFit();
                return;
            }

            ZoomLevel = 1;
        }

        void AnimateZoomLevel(float zoomLevel)
        {
            ((ImageContentView)Animator).SetValueForKey((NSNumber)zoomLevel, (NSString)ZOOM_LEVEL_KEY);
        }

        [Export(ZOOM_LEVEL_KEY)]
        float ZoomLevel
        {
            get
            {
                return mZoomLevel;
            }
            set
            {
                if (value < 0.1f)
                    return;

                mZoomLevel = value;

                OnZoomLevelChanged(mZoomLevel);
            }
        }

        void OnZoomLevelChanged(float zoomLevel)
        {
            CGSize imageSize = mImage.Size;

            CGSize targetSize = new CGSize(
                imageSize.Width * zoomLevel, imageSize.Height * zoomLevel);

            mImageView.SetFrameSize(targetSize);
        }

        float GetZoomValueToFit()
        {
            CGSize imageSize = mImage.Size;

            return ImageDiffExtensions.CalculateZoomToFit(
                imageSize.Width, imageSize.Height,
                ContentSize.Width, ContentSize.Height);
        }

        void BuildComponents()
        {
            HasVerticalScroller = true;
            HasHorizontalScroller = true;
            AutoresizingMask =
                NSViewResizingMask.WidthSizable | NSViewResizingMask.HeightSizable;

            mImageView = new ImageView();

            DocumentView = mImageView;
        }

        [Export("defaultAnimationForKey:")]
        static new NSObject DefaultAnimationFor(NSString key)
        {
            if (key == ZOOM_LEVEL_KEY)
            {
                if (mZoomAnimation == null)
                {
                    mZoomAnimation = new CABasicAnimation();
                    mZoomAnimation.Duration = 0.2f;
                }

                return mZoomAnimation;
            }

            return NSView.DefaultAnimationFor(key);
        }

        NSImage mImage;

        float mZoomLevel;

        ImageView mImageView;

        static CABasicAnimation mZoomAnimation;

        const string ZOOM_LEVEL_KEY = "zoomLevel";
        const float ZOOM_STEP = 0.3f;

    }
}

