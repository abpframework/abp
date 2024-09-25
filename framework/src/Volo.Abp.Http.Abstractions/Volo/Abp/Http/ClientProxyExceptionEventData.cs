namespace Volo.Abp.Http;

public class ClientProxyExceptionEventData
{
    public int? StatusCode { get; set; }

    public string? ReasonPhrase { get; set; }

    public string? Error { get; set; }

    public string? ErrorDescription { get; set; }

    public string? ErrorUri { get; set; }
}
