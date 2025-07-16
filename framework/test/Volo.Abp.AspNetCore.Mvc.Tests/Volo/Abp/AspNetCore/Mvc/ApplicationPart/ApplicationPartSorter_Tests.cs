using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using NSubstitute;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationPart;

public class ApplicationPartSorter_Tests
{
    [Fact]
    public void Should_Sort_ApplicationParts_By_Module_Dependencies()
    {
        var moduleDescriptors = new List<IAbpModuleDescriptor>();
        var partManager = new ApplicationPartManager();

        for (var i = 0; i < 10; i++)
        {
            var assembly = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName($"ModuleA{i}.dll"), AssemblyBuilderAccess.Run);
            partManager.ApplicationParts.Add(new AssemblyPart(assembly));
            moduleDescriptors.Add(CreateModuleDescriptor(assembly));
        }
        var randomApplicationParts = partManager.ApplicationParts.OrderBy(x => Guid.NewGuid()).ToList(); // Shuffle the parts

        // Additional part
        randomApplicationParts.AddFirst(new CompiledRazorAssemblyPart(typeof(AbpAspNetCoreModule).Assembly));
        randomApplicationParts.Insert(5, new CompiledRazorAssemblyPart(typeof(AbpAspNetCoreMvcModule).Assembly));
        randomApplicationParts.AddLast(new CompiledRazorAssemblyPart(typeof(AbpVirtualFileSystemModule).Assembly));

        partManager.ApplicationParts.Clear();
        foreach (var part in randomApplicationParts)
        {
            partManager.ApplicationParts.Add(part);
        }

        var moduleContainer = CreateFakeModuleContainer(moduleDescriptors);

        ApplicationPartSorter.Sort(partManager, moduleContainer);

        // Act
        partManager.ApplicationParts.Count.ShouldBe(13); // 10 modules + 3 additional parts

        var applicationParts = partManager.ApplicationParts.Reverse().ToList(); // Reverse the order to match the expected output

        applicationParts[0].ShouldBeOfType<CompiledRazorAssemblyPart>().Assembly.ShouldBe(typeof(AbpAspNetCoreModule).Assembly);
        applicationParts[1].ShouldBeOfType<AssemblyPart>().Assembly.GetName().Name.ShouldStartWith("ModuleA0");
        applicationParts[2].ShouldBeOfType<AssemblyPart>().Assembly.GetName().Name.ShouldStartWith("ModuleA1");
        applicationParts[3].ShouldBeOfType<AssemblyPart>().Assembly.GetName().Name.ShouldStartWith("ModuleA2");
        applicationParts[4].ShouldBeOfType<AssemblyPart>().Assembly.GetName().Name.ShouldStartWith("ModuleA3");
        applicationParts[5].ShouldBeOfType<CompiledRazorAssemblyPart>().Assembly.ShouldBe(typeof(AbpAspNetCoreMvcModule).Assembly);
        applicationParts[6].ShouldBeOfType<AssemblyPart>().Assembly.GetName().Name.ShouldStartWith("ModuleA4");
        applicationParts[7].ShouldBeOfType<AssemblyPart>().Assembly.GetName().Name.ShouldStartWith("ModuleA5");
        applicationParts[8].ShouldBeOfType<AssemblyPart>().Assembly.GetName().Name.ShouldStartWith("ModuleA6");
        applicationParts[9].ShouldBeOfType<AssemblyPart>().Assembly.GetName().Name.ShouldStartWith("ModuleA7");
        applicationParts[10].ShouldBeOfType<AssemblyPart>().Assembly.GetName().Name.ShouldStartWith("ModuleA8");
        applicationParts[11].ShouldBeOfType<AssemblyPart>().Assembly.GetName().Name.ShouldStartWith("ModuleA9");
        applicationParts[12].ShouldBeOfType<CompiledRazorAssemblyPart>().Assembly.ShouldBe(typeof(AbpVirtualFileSystemModule).Assembly);
    }

    private static IModuleContainer CreateFakeModuleContainer(List<IAbpModuleDescriptor> moduleDescriptors)
    {
        var fakeModuleContainer = Substitute.For<IModuleContainer>();
        fakeModuleContainer.Modules.Returns(moduleDescriptors);
        return fakeModuleContainer;
    }

    private static IAbpModuleDescriptor CreateModuleDescriptor(Assembly assembly)
    {
        var moduleDescriptor = Substitute.For<IAbpModuleDescriptor>();
        moduleDescriptor.Assembly.Returns(assembly);
        return moduleDescriptor;
    }
}
