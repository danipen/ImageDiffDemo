using System;

namespace ImageDiffDemo.Common
{
    public interface IPlasticTimerBuilder
    {
        IPlasticTimer Get(
            bool bModalMode,
            ThreadWaiter.TimerTick timerTickDelegate);

        IPlasticTimer Get(
            bool bModalMode,
            int timerIntervalMilliseconds,
            ThreadWaiter.TimerTick timerTickDelegate);
    }

    public interface IPlasticTimer
    {
        void Start();
        void Stop();
    }

    public interface IThreadWaiterBuilder
    {
        IThreadWaiter GetWaiter();
        IThreadWaiter GetWaiter(int timerIntervalMilliseconds);
        IThreadWaiter GetModalWaiter();
    }

    public interface IThreadWaiter
    {
        Exception Exception { get; }

        void Execute(
            PlasticThread.Operation threadOperationDelegate,
            PlasticThread.Operation afterOperationDelegate);

        void Execute(
            PlasticThread.Operation threadOperationDelegate,
            PlasticThread.Operation afterOperationDelegate,
            PlasticThread.Operation timerTickDelegate);

        void Cancel();
    }

    public static class ThreadWaiter
    {
        public delegate void TimerTick();

        public static void Initialize(IThreadWaiterBuilder threadWaiterBuilder)
        {
            mThreadWaiterBuilder = threadWaiterBuilder;
        }

        public static IThreadWaiter GetWaiter()
        {
            return mThreadWaiterBuilder.GetWaiter();
        }

        public static IThreadWaiter GetWaiter(int timerIntervalMilliseconds)
        {
            return mThreadWaiterBuilder.GetWaiter(timerIntervalMilliseconds);
        }

        public static IThreadWaiter GetModalWaiter()
        {
            return mThreadWaiterBuilder.GetModalWaiter();
        }

        public static void Reset()
        {
            mThreadWaiterBuilder = new ThreadWaiterBuilder();
        }

        static IThreadWaiterBuilder mThreadWaiterBuilder = new ThreadWaiterBuilder();
    }

    public class ThreadWaiterBuilder : IThreadWaiterBuilder
    {
        public static void Initialize(IPlasticTimerBuilder timerBuilder)
        {
            mPlasticTimerBuilder = timerBuilder;
        }

        IThreadWaiter IThreadWaiterBuilder.GetWaiter()
        {
            return new PlasticThreadWaiter(mPlasticTimerBuilder, false);
        }

        IThreadWaiter IThreadWaiterBuilder.GetWaiter(int timerIntervalMilliseconds)
        {
            return new PlasticThreadWaiter(mPlasticTimerBuilder, false, timerIntervalMilliseconds);
        }

        IThreadWaiter IThreadWaiterBuilder.GetModalWaiter()
        {
            return new PlasticThreadWaiter(mPlasticTimerBuilder, true);
        }

        static IPlasticTimerBuilder mPlasticTimerBuilder;
    }

    public class PlasticThreadWaiter : IThreadWaiter
    {
        public Exception Exception { get { return mThreadOperation.Exception; } }

        internal PlasticThreadWaiter(
            IPlasticTimerBuilder timerBuilder, bool bModalMode)
        {
            mPlasticTimer = timerBuilder.Get(bModalMode, OnTimerTick);
        }

        internal PlasticThreadWaiter(
            IPlasticTimerBuilder timerBuilder,
            bool bModalMode,
            int timerIntervalMilliseconds)
        {
            mPlasticTimer = timerBuilder.Get(bModalMode, timerIntervalMilliseconds, OnTimerTick);
        }

        public void Execute(
            PlasticThread.Operation threadOperationDelegate,
            PlasticThread.Operation afterOperationDelegate)
        {
            Execute(threadOperationDelegate, afterOperationDelegate, null);
        }

        public void Execute(
            PlasticThread.Operation threadOperationDelegate,
            PlasticThread.Operation afterOperationDelegate,
            PlasticThread.Operation timerTickDelegate)
        {
            mThreadOperation = new PlasticThread(threadOperationDelegate);
            mAfterOperationDelegate = afterOperationDelegate;
            mTimerTickDelegate = timerTickDelegate;

            mPlasticTimer.Start();

            mThreadOperation.Execute();
        }

        public void Cancel()
        {
            mbCancelled = true;
        }

        void OnTimerTick()
        {
            if (mThreadOperation.IsRunning)
            {
                if (mTimerTickDelegate != null)
                    mTimerTickDelegate();

                return;
            }

            mPlasticTimer.Stop();

            if (mbCancelled)
                return;

            mAfterOperationDelegate();
        }

        bool mbCancelled = false;

        IPlasticTimer mPlasticTimer;
        PlasticThread mThreadOperation;
        PlasticThread.Operation mTimerTickDelegate;
        PlasticThread.Operation mAfterOperationDelegate;
    }
}