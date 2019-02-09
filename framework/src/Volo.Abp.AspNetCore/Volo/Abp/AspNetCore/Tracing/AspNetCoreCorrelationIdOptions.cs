namespace Volo.Abp.AspNetCore.Tracing
{
    public class AspNetCoreCorrelationIdOptions
    {
        public string HeaderName { get; set; } = "X-Correlation-Id";
    }
}
