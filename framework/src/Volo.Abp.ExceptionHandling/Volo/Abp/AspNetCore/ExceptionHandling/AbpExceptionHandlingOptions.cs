namespace Volo.Abp.AspNetCore.ExceptionHandling
{
    public class AbpExceptionHandlingOptions
    {
        public bool SendExceptionsDetailsToClients { get; set; }
     
        private bool _enableStackTrace;

        public bool EnableStackTrace
        {
            get => _enableStackTrace;
            set
            {
                _enableStackTrace = value;
                if (_enableStackTrace)
                {
                    SendExceptionsDetailsToClients = true;
                }
            }
        }
    }
}
