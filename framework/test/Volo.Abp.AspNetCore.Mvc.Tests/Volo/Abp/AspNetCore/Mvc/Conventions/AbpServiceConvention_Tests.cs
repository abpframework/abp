using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Options;
using Shouldly;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Http.ProxyScripting.Generators;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Conventions;

public class AbpServiceConvention_Tests : AspNetCoreMvcTestBase
{
    private readonly IConventionalRouteBuilder _conventionalRouteBuilder;
    private readonly IOptions<AbpAspNetCoreMvcOptions> _options;

    public AbpServiceConvention_Tests()
    {
        _conventionalRouteBuilder = GetRequiredService<IConventionalRouteBuilder>();
        _options = GetRequiredService<IOptions<AbpAspNetCoreMvcOptions>>();
    }

    [Fact]
    public void Should_Not_Remove_Derived_Controller_If_Not_Expose_Service()
    {
        // Arrange
        var applicationModel = new ApplicationModel();
        var baseControllerModel = new ControllerModel(typeof(BaseController).GetTypeInfo(), Array.Empty<object>())
        {
            Application = applicationModel
        };
        applicationModel.Controllers.Add(baseControllerModel);

        var derivedControllerModel = new ControllerModel(typeof(DerivedController).GetTypeInfo(), Array.Empty<object>())
        {
            Application = applicationModel
        };
        applicationModel.Controllers.Add(derivedControllerModel);

        var abpServiceConvention = new AbpServiceConvention(_options, _conventionalRouteBuilder);

        // Act
        abpServiceConvention.Apply(applicationModel);

        // Assert
        applicationModel.Controllers.ShouldContain(baseControllerModel);
        applicationModel.Controllers.ShouldContain(derivedControllerModel);
    }

    [Fact]
    public void Should_Remove_Exposed_Controller_If_Expose_Self()
    {
        // Arrange
        var applicationModel = new ApplicationModel();
        var baseControllerModel = new ControllerModel(typeof(BaseController).GetTypeInfo(), Array.Empty<object>())
        {
            Application = applicationModel
        };
        applicationModel.Controllers.Add(baseControllerModel);

        var derivedControllerModel = new ControllerModel(typeof(ExposeServiceIncludeSelfDerivedController).GetTypeInfo(), Array.Empty<object>())
        {
            Application = applicationModel
        };
        applicationModel.Controllers.Add(derivedControllerModel);

        var abpServiceConvention = new AbpServiceConvention(_options, _conventionalRouteBuilder);

        // Act
        abpServiceConvention.Apply(applicationModel);

        // Assert
        applicationModel.Controllers.ShouldNotContain(baseControllerModel);
        applicationModel.Controllers.ShouldContain(derivedControllerModel);
    }

    [Fact]
    public void Should_Not_Remove_Derived_Controller_If_No_Base_Controller_Model()
    {
        // Arrange
        var applicationModel = new ApplicationModel();
        var derivedControllerModel = new ControllerModel(typeof(ExposeServiceDerivedController).GetTypeInfo(), Array.Empty<object>())
        {
            Application = applicationModel
        };
        applicationModel.Controllers.Add(derivedControllerModel);

        var abpServiceConvention = new AbpServiceConvention(_options, _conventionalRouteBuilder);

        // Act
        abpServiceConvention.Apply(applicationModel);

        // Assert
        applicationModel.Controllers.ShouldContain(derivedControllerModel);
    }

    [Fact]
    public void Should_Remove_Derived_Controller_If_Expose_Service()
    {
        // Arrange
        var applicationModel = new ApplicationModel();
        var baseControllerModel = new ControllerModel(typeof(BaseController).GetTypeInfo(), Array.Empty<object>())
        {
            Application = applicationModel
        };
        applicationModel.Controllers.Add(baseControllerModel);

        var derivedControllerModel = new ControllerModel(typeof(ExposeServiceDerivedController).GetTypeInfo(), Array.Empty<object>())
        {
            Application = applicationModel
        };
        applicationModel.Controllers.Add(derivedControllerModel);

        var abpServiceConvention = new AbpServiceConvention(_options, _conventionalRouteBuilder);

        // Act
        abpServiceConvention.Apply(applicationModel);

        // Assert
        applicationModel.Controllers.ShouldContain(baseControllerModel);
        applicationModel.Controllers.ShouldNotContain(derivedControllerModel);
    }

    [Fact]
    public async Task Parameters_Should_Binding_From_Path()
    {
        var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>("/api/abp/api-definition");
        model.ShouldNotBeNull();

        var parameters = model.Modules["test"].Controllers.First().Value.Actions.First().Value.Parameters;
        parameters.Count.ShouldBe(2);
        parameters.ShouldContain(x => x.Name == "id");
        parameters.ShouldContain(x => x.Name == "assignToId" && x.IsOptional);
        parameters.All(x => x.BindingSourceId == ParameterBindingSources.Path).ShouldBeTrue();
    }
}

public class BaseController : Controller
{
}

public class DerivedController : BaseController
{
}

[ExposeServices(typeof(BaseController))]
public class ExposeServiceDerivedController : BaseController
{
}

[ExposeServices(typeof(BaseController), IncludeSelf = true)]
public class ExposeServiceIncludeSelfDerivedController : BaseController
{
}
