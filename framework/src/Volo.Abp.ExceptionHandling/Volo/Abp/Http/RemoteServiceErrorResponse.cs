using Volo.Abp.ExceptionHandling;

namespace Volo.Abp.Http;

public class RemoteServiceErrorResponse
{
    public ErrorInfo Error { get; set; }

    public RemoteServiceErrorResponse(ErrorInfo error)
    {
        Error = error;
    }
}
