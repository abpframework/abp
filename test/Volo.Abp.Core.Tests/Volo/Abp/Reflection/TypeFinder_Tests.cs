using System.Collections.Generic;
using System.Reflection;
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
                typeof(TypeFinder_Tests).Assembly
            });

            //Act

            var typeFinder = new TypeFinder(fakeAssemblyFinder);

            //Assert

            typeFinder.Types.ShouldContain(typeof(TypeFinder_Tests));
        }
    }
}
