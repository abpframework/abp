using System;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.VirtualFileSystem;

public class DynamicFileProvider_Tests : AbpIntegratedTest<DynamicFileProvider_Tests.TestModule>
{
    private readonly IDynamicFileProvider _dynamicFileProvider;

    public DynamicFileProvider_Tests()
    {
        _dynamicFileProvider = GetRequiredService<IDynamicFileProvider>();
    }

    [Fact]
    public void Should_Get_Created_Files()
    {
        const string fileContent = "Hello World";

        _dynamicFileProvider.AddOrUpdate(
            new InMemoryFileInfo(
                "/my-files/test.txt",
                fileContent.GetBytes(),
                "test.txt"
            )
        );

        var fileInfo = _dynamicFileProvider.GetFileInfo("/my-files/test.txt");
        fileInfo.ShouldNotBeNull();
        fileInfo.ReadAsString().ShouldBe(fileContent);
    }

    [Fact]
    public void Should_Get_Notified_On_File_Change()
    {
        //Create a dynamic file

        _dynamicFileProvider.AddOrUpdate(
            new InMemoryFileInfo(
                "/my-files/test.txt",
                "Hello World".GetBytes(),
                "test.txt"
            )
        );

        //Register to change on that file

        var fileCallbackCalled = false;

        ChangeToken.OnChange(
            () => _dynamicFileProvider.Watch("/my-files/test.txt"),
            () => { fileCallbackCalled = true; });

        //Updating the file should trigger the callback

        _dynamicFileProvider.AddOrUpdate(
            new InMemoryFileInfo(
                "/my-files/test.txt",
                "Hello World UPDATED".GetBytes(),
                "test.txt"
            )
        );

        fileCallbackCalled.ShouldBeTrue();

        //Updating the file should trigger the callback (2nd test)

        fileCallbackCalled = false;

        _dynamicFileProvider.AddOrUpdate(
            new InMemoryFileInfo(
                "/my-files/test.txt",
                "Hello World UPDATED 2".GetBytes(),
                "test.txt"
            )
        );

        fileCallbackCalled.ShouldBeTrue();
    }

    [DependsOn(typeof(AbpVirtualFileSystemModule))]
    public class TestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

        }
    }
}
