using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Auditing;
using Volo.Abp.TestApp.Application;

namespace Volo.Abp.AspNetCore.Mvc.Auditing;

[Route("api/streaming-json-test")]
public class StreamingJsonTestController : AbpController
{
    private readonly StreamingAppService _streamingAppService;

    public StreamingJsonTestController(StreamingAppService streamingAppService)
    {
        _streamingAppService = streamingAppService;
    }

    [HttpGet]
    [Audited]
    public IActionResult Get()
    {
        return Json(_streamingAppService.GetItemsAsync());
    }

    [HttpGet]
    [Route("handled-exception")]
    [Audited]
    [ReturnStreamOnException]
    public IActionResult GetWithHandledException()
    {
        throw new InvalidOperationException("Handled by the filter");
    }

    [HttpGet]
    [Route("creation-error")]
    public IActionResult GetWithCreationError()
    {
        return Json(_streamingAppService.GetItemsWithCreationErrorAsync());
    }
}

public class ReturnStreamOnExceptionAttribute : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception == null)
        {
            return;
        }

        context.ExceptionHandled = true;
        context.Result = new ObjectResult(context.HttpContext.RequestServices.GetRequiredService<StreamingAppService>().GetItemsAsync());
    }
}
