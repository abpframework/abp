namespace Volo.Abp.Tracing
{
    public class CorrelationIdOptions
    {
        public string HttpHeaderName { get; set; } = "X-Correlation-Id";
    }
}
