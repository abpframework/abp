namespace Volo.Abp.AspNetCore.Serilog
{
    public class AbpAspNetCoreSerilogEnrichersOptions
    {
        public string TenantIdEnricherPropertyName { get; set; }

        public string UserIdEnricherPropertyName { get; set; }

        public string ClientIdEnricherPropertyName { get; set; }

        public string CorrelationIdPropertyName { get; set; }

        public AbpAspNetCoreSerilogEnrichersOptions()
        {
            TenantIdEnricherPropertyName = AbpSerilogEnrichersConsts.TenantIdEnricherPropertyName;
            UserIdEnricherPropertyName = AbpSerilogEnrichersConsts.UserIdEnricherPropertyName;
            ClientIdEnricherPropertyName = AbpSerilogEnrichersConsts.ClientIdEnricherPropertyName;
            CorrelationIdPropertyName = AbpSerilogEnrichersConsts.CorrelationIdPropertyName;
        }
    }
}