using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Shouldly;
using Volo.Abp.AspNetCore.Mvc.Auditing;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc.Features;
using Volo.Abp.AspNetCore.Mvc.GlobalFeatures;
using Volo.Abp.AspNetCore.Mvc.Response;
using Volo.Abp.AspNetCore.Mvc.Uow;
using Volo.Abp.AspNetCore.Mvc.Validation;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Filters;

[Route("api/enabled-features-test")]
public class EnabledAbpFeaturesController : AbpController, IRemoteService
{
    [HttpGet]
    public Task<List<string>> GetAsync()
    {
        var filters = HttpContext.GetEndpoint().Metadata.GetMetadata<ControllerActionDescriptor>()
            .FilterDescriptors.Where(x => x.Filter is ServiceFilterAttribute)
            .Select(x => x.Filter.As<ServiceFilterAttribute>().ServiceType.FullName).ToList();

        return Task.FromResult(filters);
    }
}

[Route("api/disabled-features-test")]
[DisableAbpFeatures]
public class DisabledAbpFeaturesController : AbpController, IRemoteService
{
    [HttpGet]
    public Task<List<string>> GetAsync()
    {
        var filters = HttpContext.GetEndpoint().Metadata.GetMetadata<ControllerActionDescriptor>()
            .FilterDescriptors.Where(x => x.Filter is ServiceFilterAttribute)
            .Select(x => x.Filter.As<ServiceFilterAttribute>().ServiceType.FullName).ToList();

        return Task.FromResult(filters);
    }
}

public class MvcDisableAbpFeaturesAttribute_Tests : AspNetCoreMvcTestBase
{
    [Fact]
    public async Task Should_Disable_MVC_Filters()
    {
        var filters = await GetResponseAsObjectAsync<List<string>>("/api/enabled-features-test");
        filters.ShouldContain(typeof(GlobalFeatureActionFilter).FullName);
        filters.ShouldContain(typeof(AbpAuditActionFilter).FullName);
        filters.ShouldContain(typeof(AbpNoContentActionFilter).FullName);
        filters.ShouldContain(typeof(AbpFeatureActionFilter).FullName);
        filters.ShouldContain(typeof(AbpValidationActionFilter).FullName);
        filters.ShouldContain(typeof(AbpUowActionFilter).FullName);
        filters.ShouldContain(typeof(AbpExceptionFilter).FullName);

        filters = await GetResponseAsObjectAsync<List<string>>("/api/disabled-features-test");
        filters.ShouldNotContain(typeof(GlobalFeatureActionFilter).FullName);
        filters.ShouldNotContain(typeof(AbpAuditActionFilter).FullName);
        filters.ShouldNotContain(typeof(AbpNoContentActionFilter).FullName);
        filters.ShouldNotContain(typeof(AbpFeatureActionFilter).FullName);
        filters.ShouldNotContain(typeof(AbpValidationActionFilter).FullName);
        filters.ShouldNotContain(typeof(AbpUowActionFilter).FullName);
        filters.ShouldNotContain(typeof(AbpExceptionFilter).FullName);
    }

}
