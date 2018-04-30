using Foundation;
using ImageDiffDemo.Common;
using System;

namespace ImageDiffDemo
{
    internal class MacPlasticTimerBuilder : IPlasticTimerBuilder
    {
        public IPlasticTimer Get(bool bModalMode, ThreadWaiter.TimerTick timerTickDelegate)
        {
            return new MacPlasticTimer(
                bModalMode, 0.05, timerTickDelegate);
        }

        public IPlasticTimer Get(bool bModalMode, int timerInterval, ThreadWaiter.TimerTick timerTickDelegate)
        {
            return new MacPlasticTimer(bModalMode, timerInterval / 1000f, timerTickDelegate);
        }
    }

    internal class MacPlasticTimer : IPlasticTimer
    {
        internal static void SetTestingMode()
        {
            mbIsTestingMode = true;
        }

        internal MacPlasticTimer(bool bModalMode, double timerInterval, ThreadWaiter.TimerTick timerTickDelegate)
        {
            mbModalMode = bModalMode;
            mTimerInterval = timerInterval;
            mTimerTickDelegate = timerTickDelegate;
        }

        public void Start()
        {
            mTimer = NSTimer.CreateRepeatingScheduledTimer(mTimerInterval, OnTimerTick);

            NSRunLoop.Current.AddTimer(mTimer, GetRunLoopMode());
        }

        NSRunLoopMode GetRunLoopMode()
        {
            if (mbIsTestingMode)
                return NSRunLoopMode.Common;
            return mbModalMode ? NSRunLoopMode.ModalPanel : NSRunLoopMode.EventTracking;
        }

        public void Stop()
        {
            if (mTimer == null)
                return;

            mTimer.Invalidate();
            mTimer = null;
        }

        void OnTimerTick(NSTimer tr)
        {
            try
            {
                mTimerTickDelegate();
            }
            catch (Exception ex)
            {
                
            }
        }

        double mTimerInterval;
        NSTimer mTimer;
        ThreadWaiter.TimerTick mTimerTickDelegate;
        bool mbModalMode;

        static bool mbIsTestingMode = false;
    }
}
