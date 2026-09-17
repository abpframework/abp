using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.ApiExploring;

public class AbpRemoteServiceApiDescriptionProvider_Tests
{
    private static (AbpRemoteServiceApiDescriptionProvider provider,
                    AbpRemoteServiceApiDescriptionProviderOptions options)
        CreateProvider()
    {
        var options = new AbpRemoteServiceApiDescriptionProviderOptions();
        options.SupportedResponseTypes.Add(new ApiResponseType
        {
            Type = typeof(string),
            StatusCode = 400
        });
        options.SupportedResponseTypes.Add(new ApiResponseType
        {
            Type = typeof(string),
            StatusCode = 500
        });

        var mvcOptions = new MvcOptions();
        mvcOptions.OutputFormatters.Add(new FakeOutputFormatter());

        var provider = new AbpRemoteServiceApiDescriptionProvider(
            new EmptyModelMetadataProvider(),
            new OptionsWrapper<MvcOptions>(mvcOptions),
            new OptionsWrapper<AbpRemoteServiceApiDescriptionProviderOptions>(options));

        return (provider, options);
    }

    private static ApiDescriptionProviderContext CreateContext()
    {
        var actionDescriptor = new ControllerActionDescriptor
        {
            ControllerName = "Fake",
            ActionName = "Get",
            ControllerTypeInfo = typeof(FakeRemoteController).GetTypeInfo(),
            MethodInfo = typeof(FakeRemoteController).GetMethod(nameof(FakeRemoteController.Get))!,
            AttributeRouteInfo = new AttributeRouteInfo { Template = "api/fake" }
        };

        var description = new ApiDescription
        {
            ActionDescriptor = actionDescriptor,
            HttpMethod = "GET",
            RelativePath = "api/fake"
        };

        return new ApiDescriptionProviderContext(new[] { actionDescriptor })
        {
            Results = { description }
        };
    }

    [Fact]
    public void Should_Not_Mutate_Shared_Options_On_Repeated_Calls()
    {
        var (provider, options) = CreateProvider();

        provider.OnProvidersExecuting(CreateContext());
        provider.OnProvidersExecuting(CreateContext());
        provider.OnProvidersExecuting(CreateContext());

        foreach (var template in options.SupportedResponseTypes)
        {
            template.ApiResponseFormats.ShouldBeEmpty();
        }
    }

    [Fact]
    public void Should_Add_Response_Types_With_Preserved_Template_Fields_And_Formats()
    {
        var options = new AbpRemoteServiceApiDescriptionProviderOptions();
        options.SupportedResponseTypes.Add(new ApiResponseType
        {
            Type = typeof(string),
            StatusCode = 418,
            Description = "I'm a teapot",
            IsDefaultResponse = true
        });

        var mvcOptions = new MvcOptions();
        mvcOptions.OutputFormatters.Add(new FakeOutputFormatter());

        var provider = new AbpRemoteServiceApiDescriptionProvider(
            new EmptyModelMetadataProvider(),
            new OptionsWrapper<MvcOptions>(mvcOptions),
            new OptionsWrapper<AbpRemoteServiceApiDescriptionProviderOptions>(options));

        var context = CreateContext();
        provider.OnProvidersExecuting(context);

        var result = context.Results.Single();
        var added = result.SupportedResponseTypes.ShouldHaveSingleItem();
        added.StatusCode.ShouldBe(418);
        added.Type.ShouldBe(typeof(string));
        added.Description.ShouldBe("I'm a teapot");
        added.IsDefaultResponse.ShouldBeTrue();
        added.ModelMetadata.ShouldNotBeNull();
        added.ApiResponseFormats.Select(x => x.MediaType)
            .ShouldBe(new[] { "application/json", "text/json" });
    }

    [Fact]
    public void Should_Not_Throw_Under_Concurrent_Calls()
    {
        var (provider, _) = CreateProvider();

        var exception = Record.Exception(() =>
            Parallel.For(0, 8, _ =>
            {
                for (var i = 0; i < 500; i++)
                {
                    provider.OnProvidersExecuting(CreateContext());
                }
            }));

        exception.ShouldBeNull();
    }

    private sealed class FakeRemoteController : IRemoteService
    {
        public string Get() => string.Empty;
    }

    private sealed class FakeOutputFormatter : IOutputFormatter, IApiResponseTypeMetadataProvider
    {
        public bool CanWriteResult(OutputFormatterCanWriteContext context) => true;

        public Task WriteAsync(OutputFormatterWriteContext context) => Task.CompletedTask;

        public IReadOnlyList<string> GetSupportedContentTypes(string? contentType, Type objectType)
            => new[] { "application/json", "text/json" };
    }
}
