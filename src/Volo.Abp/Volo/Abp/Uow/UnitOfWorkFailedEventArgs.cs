using System;
using JetBrains.Annotations;

namespace Volo.Abp.Uow
{
    /// <summary>
    /// Used as event arguments on <see cref="IUnitOfWork.Failed"/> event.
    /// </summary>
    public class UnitOfWorkFailedEventArgs : EventArgs
    {
        /// <summary>
        /// Exception that caused failure. This is set only if an error occured during <see cref="IBasicUnitOfWork.Complete"/>.
        /// Can be null if there is no exception, but <see cref="IBasicUnitOfWork.Complete"/> is not called. 
        /// Can be null if another exception occurred during the UOW.
        /// </summary>
        [CanBeNull]
        public Exception Exception { get; private set; }

        /// <summary>
        /// Creates a new <see cref="UnitOfWorkFailedEventArgs"/> object.
        /// </summary>
        /// <param name="exception">Exception that caused failure</param>
        public UnitOfWorkFailedEventArgs([CanBeNull] Exception exception)
        {
            Exception = exception;
        }
    }
}
