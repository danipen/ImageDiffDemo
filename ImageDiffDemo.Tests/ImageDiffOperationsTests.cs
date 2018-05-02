using NUnit.Framework;
using Moq;
using System;
using ImageDiffDemo.Common;
using System.Runtime.InteropServices;

namespace ImageDiffDemo.Tests
{
    [TestFixture()]
    public class Test
    {
        [SetUp]
        public void Setup()
        {
            ThreadWaiter.Initialize(new TestingThreadWaiterBuilder());
        }

        [TearDown]
        public void TearDown()
        {
            ThreadWaiter.Reset();
        }


        [Test()]
        public unsafe void TestImageDiffDifferentSizes()
        {
            Mock<IImageDiffView> diffViewMock = new Mock<IImageDiffView>();
            Mock<IProgressControls> progressControlsMock = new Mock<IProgressControls>();

            ImageDiffOperations.CalculateImageDiff(
                diffViewMock.Object, progressControlsMock.Object,
                null, null, 256, 512);

            progressControlsMock.Verify(
                progress => progress.ShowInformation(ImageDiffOperations.DIFF_SIZE_INFO_MESSSAGE),
                Times.Once(),
                "Info message about diff image sizes should be notified.");

            diffViewMock.Verify(
                diffView => diffView.SetDiffImageBytes(It.IsAny<byte[]>()),
                Times.Never(),
                "SetDiffImageBytes should not be called.");
        }

        [Test()]
        public unsafe void TestImageDiffsSucessCalculation()
        {
            Mock<IImageDiffView> diffViewMock = new Mock<IImageDiffView>();
            Mock<IProgressControls> progressControlsMock = new Mock<IProgressControls>();

            byte[] leftBitmap = new byte[0];
            byte[] rightBitmap = new byte[0];

            fixed (byte* leftPointer = leftBitmap)
            fixed (byte* rightPointer = rightBitmap)
            {
                ImageDiffOperations.CalculateImageDiff(
                    diffViewMock.Object, progressControlsMock.Object,
                    leftPointer, rightPointer, 0, 0);
            }

            progressControlsMock.Verify(
                progress => progress.ShowProgress(),
                Times.Once(),
                "Progress should be displayed");

            progressControlsMock.Verify(
                progress => progress.HideProgress(),
                Times.Once(),
                "Progress should be hidden");

            diffViewMock.Verify(
                diffView => diffView.SetDiffImageBytes(It.IsAny<byte[]>()),
                Times.Once(),
                "SetDiffImageBytes should be called.");
        }
    }
}
