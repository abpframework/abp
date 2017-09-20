namespace Volo.Abp.Http
{
    public class RemoteServiceErrorResponse
    {
        public RemoteServiceErrorInfo Error { get; set; }

        /// <summary>
        /// A special signature of ABP.
        /// </summary>
        public bool __abp { get; } = true;

        public RemoteServiceErrorResponse(RemoteServiceErrorInfo error)
        {
            Error = error;
        }
    }
}