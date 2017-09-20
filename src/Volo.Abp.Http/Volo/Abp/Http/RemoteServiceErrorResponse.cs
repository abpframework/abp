namespace Volo.Abp.Http
{
    public abstract class RemoteServiceErrorResponse
    {
        public RemoteServiceErrorInfo Error { get; set; }

        /// <summary>
        /// A special signature of ABP.
        /// </summary>
        public bool __abp { get; } = true;
    }
}