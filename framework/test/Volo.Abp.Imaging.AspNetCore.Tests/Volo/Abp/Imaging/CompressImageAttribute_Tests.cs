using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.Imaging;

public class CompressImageAttribute_Tests : AbpImagingAspNetCoreTestBase
{
    [Fact]
    public async Task Should_Compressed_IFormFile()
    {
        var attribute = new CompressImageAttribute();
        
        var serviceScopeFactory = GetRequiredService<IServiceScopeFactory>();

        await using var stream = ImageFileHelper.GetJpgTestFileStream();
        
        var actionExecutingContext = new ActionExecutingContext(
            new ActionContext() {
                HttpContext = new DefaultHttpContext() {
                    ServiceScopeFactory = serviceScopeFactory
                },
                RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
                ActionDescriptor = new ControllerActionDescriptor(),
            },
            new List<IFilterMetadata>(),
            new Dictionary<string, object>
            {
                {"file", new FormFile(stream, 0, stream.Length, "file", "test.jpg") {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg"
                }}
            },
            new object()
        );
        
        await attribute.OnActionExecutionAsync(actionExecutingContext, async () => await Task.FromResult(new ActionExecutedContext(
            actionExecutingContext,
            new List<IFilterMetadata>(),
            new object()
        )));
        
        actionExecutingContext.ActionArguments["file"].ShouldNotBeNull();
        actionExecutingContext.ActionArguments["file"].ShouldBeAssignableTo<IFormFile>();
    }
}