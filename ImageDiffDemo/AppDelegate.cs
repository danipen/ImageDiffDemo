using AppKit;
using Foundation;
using ImageDiffDemo.Common;

namespace ImageDiffDemo
{
    [Register("AppDelegate")]
    public class AppDelegate : NSApplicationDelegate
    {
        public AppDelegate()
        {
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            ThreadWaiterBuilder.Initialize(new MacPlasticTimerBuilder());

            NSWindow window = new MainWindow();
            window.MakeKeyAndOrderFront(this);
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }

        public override bool ApplicationShouldTerminateAfterLastWindowClosed(NSApplication sender)
        {
            return true;
        }
    }
}
