using System;
using System.Collections.Generic;
using System.Threading;

namespace Dispatch.Common
{
    public class SafeThread
    {
        public event Action<Exception> OnException;

        public Thread Thread { get; }
        public List<Exception> TotalExceptions { get; }
        public SafeThread(Action action)
        {
            Thread = new Thread(() => SafeExecute(action, Handler));
            TotalExceptions = new List<Exception>();
        }

        private void Handler(Exception e)
        {
            OnException?.Invoke(e);
            TotalExceptions.Add(e);

            if (IsAlive)
                Abort();
        }

        private static void SafeExecute(Action a, Action<Exception> ae)
        {
            try
            {
                a();
            }
            catch (Exception e)
            {
                ae(e);
            }
        }

        #region Implements

        public bool IsAlive => Thread.IsAlive;

        public void Start() => Thread.Start();
        public void Abort() => Thread.Abort();


        #endregion
    }
}
