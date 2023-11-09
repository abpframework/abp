namespace Volo.Abp.Http;

public class ClientProxyExceptionEventData
{
    public int? StatusCode { get; set; }

    public string? ReasonPhrase { get; set; }
}
