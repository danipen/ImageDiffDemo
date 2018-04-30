using System;
using System.Threading;

namespace ImageDiffDemo.Common
{
    public class PlasticThreadPool
    {
        public static void Run(WaitCallback callBack)
        {
            ThreadPool.QueueUserWorkItem(callBack);
        }

        public static void Run(WaitCallback callBack, object state)
        {
            ThreadPool.QueueUserWorkItem(callBack, state);
        }
    }
}