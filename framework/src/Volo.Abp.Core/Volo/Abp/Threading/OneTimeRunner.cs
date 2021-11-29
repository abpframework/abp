using System;

namespace Volo.Abp.Threading
{
    /// <summary>
    /// This class is used to ensure running of a code block only once.
    /// It can be instantiated as a static object to ensure that the code block runs only once in the application lifetime.
    /// </summary>
    public class OneTimeRunner
    {
        private volatile bool _runBefore;

        public void Run(Action action)
        {
            if (_runBefore)
            {
                return;
            }

            lock (this)
            {
                if (_runBefore)
                {
                    return;
                }

                action();

                _runBefore = true;
            }
        }
    }
}