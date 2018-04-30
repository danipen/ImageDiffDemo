namespace ImageDiffDemo.Common
{
    public interface IImageDiffView
    {
        void SetDiffImageBytes(byte[] diffImage);
    }

    public interface IProgressControls
    {
        void ShowProgress();
        void HideProgress();
        void ShowInformation(string message);
        void ShowError(string message);
    }
}
