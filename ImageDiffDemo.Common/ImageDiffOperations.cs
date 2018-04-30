using System;
namespace ImageDiffDemo.Common
{
    public static class ImageDiffOperations
    {
        public static unsafe void CalculateImageDiff(
            IImageDiffView diffView, IProgressControls progressControls,
            byte* leftBitmapData, byte* rightBitmapData, int leftLen, int rightLen)
        {
            if (leftLen != rightLen)
            {
                progressControls.ShowInformation(
                    "In order to calculate image differences, left and right images " + 
                    "must be of the same lenght.");
                return;
            }

            byte[] diffImage = null;

            //progressControls.ShowProgress("Calculating image diffs...");

            IThreadWaiter waiter = ThreadWaiter.GetWaiter();
            waiter.Execute(
                /*threadOperationDelegate*/ delegate
                {
                diffImage = PixelDiff(leftBitmapData, rightBitmapData, leftLen);
                },
                /*afterOperationDelegate*/ delegate
                {
                    //progressControls.HideProgress();

                    if (waiter.Exception != null)
                    {
                        //progressControls.ShowError(waiter.Exception.Message);
                        return;
                    }

                    diffView.SetDiffImageBytes(diffImage);
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
