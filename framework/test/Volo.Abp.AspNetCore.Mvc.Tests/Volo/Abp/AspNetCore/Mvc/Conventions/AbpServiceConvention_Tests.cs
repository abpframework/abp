using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using System;
using System.Reflection;
using Volo.Abp.DependencyInjection;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Conventions
{
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
}
