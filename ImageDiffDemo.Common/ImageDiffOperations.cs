using System;
namespace ImageDiffDemo.Common
{
    public static class ImageDiffOperations
    {
        public const string DIFF_SIZE_INFO_MESSSAGE =
            "In order to calculate image differences, left and right images " +
            "must be of the same lenght.";

        public static unsafe void CalculateImageDiff(
            IImageDiffView diffView, IProgressControls progressControls,
            byte* leftBitmapData, byte* rightBitmapData, int leftLen, int rightLen)
        {
            if (leftLen != rightLen)
            {
                progressControls.ShowInformation(DIFF_SIZE_INFO_MESSSAGE);
                return;
            }

            byte[] resultDiffImage = null;

            progressControls.ShowProgress();

            IThreadWaiter waiter = ThreadWaiter.GetWaiter();
            waiter.Execute(
                /*threadOperationDelegate*/ delegate
                {
                    resultDiffImage = PixelDiff(leftBitmapData, rightBitmapData, leftLen);
                },
                /*afterOperationDelegate*/ delegate
                {
                    progressControls.HideProgress();

                    if (waiter.Exception != null)
                    {
                        progressControls.ShowError(waiter.Exception.Message);
                        return;
                    }

                    diffView.SetDiffImageBytes(resultDiffImage);
                });
        }

        static unsafe byte[] PixelDiff(byte* leftBitmapData, byte* rightBitmapData, int len)
        {
            byte[] outputBytes = new byte[len];

            for (int i = 0; i < len; i++)
            {
                // For alpha use the average of both images (otherwise pixels with the same alpha won't be visible)
                if ((i + 1) % 4 == 0)
                    outputBytes[i] = (byte)((*leftBitmapData + *rightBitmapData) / 2);
                else
                    outputBytes[i] = (byte)~(*leftBitmapData ^ *rightBitmapData);


                leftBitmapData++;
                rightBitmapData++;
            }

            return outputBytes;
        }
    }
}
