using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Volo.Abp.Reflection
{
    public class TypeFinder_Tests
    {
        [Fact]
        public void Should_Find_Types_In_Given_Assemblies()
        {
            //Arrange

            var fakeAssemblyFinder = Substitute.For<IAssemblyFinder>();
            fakeAssemblyFinder.Assemblies.Returns(new List<Assembly>
            {
                typeof(AbpKernelModule).GetAssembly(),
                typeof(TypeFinder_Tests).GetAssembly()
            });

            //Act

            var typeFinder = new TypeFinder(fakeAssemblyFinder, Substitute.For<ILogger<TypeFinder>>());

            //Assert

            typeFinder.Types.ShouldContain(typeof(AbpKernelModule));
            typeFinder.Types.ShouldContain(typeof(TypeFinder_Tests));
        }
    }
}
