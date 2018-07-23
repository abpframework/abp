namespace Volo.Abp.Threading
{
    /// <summary>
    /// Interface to start/stop self threaded services.
    /// </summary>
    public interface IRunnable
    {
        /// <summary>
        /// Starts the service.
        /// </summary>
        void Start();

        /// <summary>
        /// Sends stop command to the service.
        /// Service may return immediately and stop asynchronously.
        /// A client should then call <see cref="WaitToStop"/> method to ensure it's stopped.
        /// </summary>
        void Stop();

        /// <summary>
        /// Waits the service to stop.
        /// </summary>
        void WaitToStop();
    }
}
