using System;

namespace Volo.Abp.Http
{
    /// <summary>
    /// Used to store information about a validation error.
    /// </summary>
    [Serializable]
    public class RemoteServiceValidationErrorInfo
    {
        /// <summary>
        /// Validation error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Relate invalid members (fields/properties).
        /// </summary>
        public string[] Members { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="RemoteServiceValidationErrorInfo"/>.
        /// </summary>
        public RemoteServiceValidationErrorInfo()
        {

        }

        /// <summary>
        /// Creates a new instance of <see cref="RemoteServiceValidationErrorInfo"/>.
        /// </summary>
        /// <param name="message">Validation error message</param>
        public RemoteServiceValidationErrorInfo(string message)
        {
            Message = message;
        }
        
        /// <summary>
        /// Creates a new instance of <see cref="RemoteServiceValidationErrorInfo"/>.
        /// </summary>
        /// <param name="message">Validation error message</param>
        /// <param name="members">Related invalid members</param>
        public RemoteServiceValidationErrorInfo(string message, string[] members)
            : this(message)
        {
            Members = members;
        }

        /// <summary>
        /// Creates a new instance of <see cref="RemoteServiceValidationErrorInfo"/>.
        /// </summary>
        /// <param name="message">Validation error message</param>
        /// <param name="member">Related invalid member</param>
        public RemoteServiceValidationErrorInfo(string message, string member)
            : this(message, new[] { member })
        {

        }
    }
}