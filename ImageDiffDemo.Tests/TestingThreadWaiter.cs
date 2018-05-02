using System;
using ImageDiffDemo.Common;

namespace ImageDiffDemo.Tests
{
    public class TestingThreadWaiterBuilder : IThreadWaiterBuilder
    {
        IThreadWaiter IThreadWaiterBuilder.GetWaiter()
        {
            return new TestingThreadWaiter();
        }

        IThreadWaiter IThreadWaiterBuilder.GetWaiter(int timerInterval)
        {
            return new TestingThreadWaiter();
        }

        IThreadWaiter IThreadWaiterBuilder.GetModalWaiter()
        {
            return new TestingThreadWaiter();
        }
    }

    public class TestingThreadWaiter : IThreadWaiter
    {
        public Exception Exception { get { return mException; } }

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
            try
            {
                threadOperationDelegate();
            }
            catch (Exception ex)
            {
                mException = ex;
            }

            if (timerTickDelegate != null)
                timerTickDelegate();

            afterOperationDelegate();
        }

        public void Cancel()
        {
        }

        Exception mException;
    }
}