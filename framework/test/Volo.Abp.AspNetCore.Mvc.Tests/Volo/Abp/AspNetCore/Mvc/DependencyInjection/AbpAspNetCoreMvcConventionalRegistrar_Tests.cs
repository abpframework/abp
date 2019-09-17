using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.DependencyInjection
{
    public class AbpAspNetCoreMvcConventionalRegistrar_Tests
    {
        [Fact]
        public void Should_Registered_Mvc_Service()
        {
            //Arrange
            var services = new ServiceCollection();

            //Act
            services.AddConventionalRegistrar(new DefaultConventionalRegistrar());
            services.AddConventionalRegistrar(new AbpAspNetCoreMvcConventionalRegistrar());

            services.AddTypes(typeof(My_Test_PageModel), typeof(My_Test_Controller), typeof(My_Test_ViewComponent));

            //Assert
            services.ShouldContainTransient(typeof(My_Test_PageModel)); 
            services.ShouldContainTransient(typeof(My_Test_Controller)); 
            services.ShouldContainTransient(typeof(My_Test_ViewComponent)); 

            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetServices<My_Test_PageModel>().Count().ShouldBe(1);
            serviceProvider.GetServices<My_Test_Controller>().Count().ShouldBe(1);
            serviceProvider.GetServices<My_Test_ViewComponent>().Count().ShouldBe(1);
        }
    }

    public class My_Test_PageModel : PageModel
    {

    }

    public class My_Test_Controller : Controller
    {

    }

    public class My_Test_ViewComponent : ViewComponent
    {

    }
}
