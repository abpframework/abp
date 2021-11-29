namespace Volo.Abp.AspNetCore.Serilog
{
    public class AbpAspNetCoreSerilogOptions
    {
        public AllEnricherPropertyNames EnricherPropertyNames { get; } = new AllEnricherPropertyNames();

        public class AllEnricherPropertyNames
        {
            /// <summary>
            /// Default value: "TenantId".
            /// </summary>
            public string TenantId { get; set; } = "TenantId";

            /// <summary>
            /// Default value: "UserId".
            /// </summary>
            public string UserId { get; set; } = "UserId";

            /// <summary>
            /// Default value: "ClientId".
            /// </summary>
            public string ClientId { get; set; } = "ClientId";

            /// <summary>
            /// Default value: "CorrelationId".
            /// </summary>
            public string CorrelationId { get; set; } = "CorrelationId";
        }
    }
}