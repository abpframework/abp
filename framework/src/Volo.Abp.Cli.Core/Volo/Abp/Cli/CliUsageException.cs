using System;

namespace Volo.Abp.Cli
{
    public class CliUsageException : Exception
    {
        public CliUsageException(string message)
            : base(message)
        {

        }

        public CliUsageException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
