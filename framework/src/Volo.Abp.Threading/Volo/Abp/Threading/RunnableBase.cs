namespace Volo.Abp.Threading
{
    /// <summary>
    /// Base implementation of <see cref="IRunnable"/>.
    /// </summary>
    public abstract class RunnableBase : IRunnable
    {
        /// <summary>
        /// A boolean value to check if this is running.
        /// </summary>
        public bool IsRunning => _isRunning;
        private volatile bool _isRunning;

        public virtual void Start()
        {
            _isRunning = true;
        }

        public virtual void Stop()
        {
            _isRunning = false;
        }

        public virtual void WaitToStop()
        {

        }
    }
}