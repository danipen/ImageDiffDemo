using System;
using System.Threading;

namespace ImageDiffDemo.Common
{
    public class PlasticThread
    {
        public bool IsRunning { get { lock (mLock) { return mbIsRunning; } } }
        public Exception Exception { get { lock (mLock) { return mException; } } }

        public delegate void Operation();

        public PlasticThread(Operation performOperationDelegate)
        {
            // Must be set out of the thread so that the timer can never
            // check it before it is changed in the thread.
            SetRunning(true);

            mPerformOperationDelegate = performOperationDelegate;
        }

        public void Execute()
        {
            PlasticThreadPool.Run(new WaitCallback(ThreadWork));
        }

        void ThreadWork(object state)
        {
            SetException(null);

            try
            {
                mPerformOperationDelegate();
            }
            catch (Exception ex)
            {
                SetException(ex);

                if (ex.InnerException == null)
                    return;

                SetException(ex.InnerException);
            }
            finally
            {
                SetRunning(false);
            }
        }

        void SetRunning(bool bIsRunning)
        {
            lock (mLock)
            {
                mbIsRunning = bIsRunning;
            }
        }

        void SetException(Exception exception)
        {
            lock (mLock)
            {
                mException = exception;
            }
        }

        Operation mPerformOperationDelegate;

        bool mbIsRunning;
        Exception mException;
        object mLock = new object();
    }
}