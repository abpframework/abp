namespace Volo.Abp.Dapr;

public class AbpDaprOptions
{
    public string AppId { get; set; }
    
    public string HttpEndpoint { get; set; }

    public string GrpcEndpoint { get; set; }
}
