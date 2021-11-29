using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.Reflection;

public class AssemblyFinder_Tests
{
    [Theory]
    [InlineData(new object[] { new Type[] { } })]
    [InlineData(new object[] { new[] { typeof(IndependentEmptyModule) } })]
    public void Should_Get_Assemblies_Of_Given_Modules(Type[] moduleTypes)
    {
        //Arrange

        var fakeModuleContainer = CreateFakeModuleContainer(moduleTypes);

        //Act

        var assemblyFinder = new AssemblyFinder(fakeModuleContainer);

        //Assert

        assemblyFinder.Assemblies.Count.ShouldBe(moduleTypes.Length);

        foreach (var moduleType in moduleTypes)
        {
            assemblyFinder.Assemblies.ShouldContain(moduleType.Assembly);
        }
    }

    private static IModuleContainer CreateFakeModuleContainer(IEnumerable<Type> moduleTypes)
    {
        var moduleDescriptors = moduleTypes.Select(CreateModuleDescriptor).ToList();
        return CreateFakeModuleContainer(moduleDescriptors);
    }

    private static IModuleContainer CreateFakeModuleContainer(List<IAbpModuleDescriptor> moduleDescriptors)
    {
        var fakeModuleContainer = Substitute.For<IModuleContainer>();
        fakeModuleContainer.Modules.Returns(moduleDescriptors);
        return fakeModuleContainer;
    }

    private static IAbpModuleDescriptor CreateModuleDescriptor(Type moduleType)
    {
        var moduleDescriptor = Substitute.For<IAbpModuleDescriptor>();
        moduleDescriptor.Type.Returns(moduleType);
        return moduleDescriptor;
    }
}
