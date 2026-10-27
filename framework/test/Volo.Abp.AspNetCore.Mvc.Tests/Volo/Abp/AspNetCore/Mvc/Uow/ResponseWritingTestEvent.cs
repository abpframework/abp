using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace Volo.Abp.AspNetCore.Mvc.Uow;

public class ResponseWritingTestEvent
{
}

/// <summary>
/// Writes to the HTTP response from a local event handler. When the event is published inside the
/// request unit of work, this runs during the unit of work completion at the end of the pipeline
/// and starts the response from inside that completion.
/// </summary>
public class ResponseWritingTestEventHandler : ILocalEventHandler<ResponseWritingTestEvent>, ITransientDependency
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ResponseWritingTestEventHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task HandleEventAsync(ResponseWritingTestEvent eventData)
    {
        var response = _httpContextAccessor.HttpContext?.Response;
        if (response != null)
        {
            await response.WriteAsync("event-written");
        }
    }
}
