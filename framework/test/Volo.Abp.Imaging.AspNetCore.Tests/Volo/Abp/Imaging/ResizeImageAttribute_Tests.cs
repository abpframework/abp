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

public class ResizeImageAttribute_Tests : AbpImagingAspNetCoreTestBase
{
    [Fact]
    public void Should_Init()
    {
        var attribute = new ResizeImageAttribute(100, 100);
        
        attribute.Width.ShouldBe(100);
        attribute.Height.ShouldBe(100);
    }
    
    [Fact]
    public async Task Should_Resized_IFormFile()
    {
        var attribute = new ResizeImageAttribute(100, 100);
        
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