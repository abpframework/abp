using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Volo.Abp.OpenIddict.Applications;

public class AbpApplicationManager_Tests : OpenIddictDomainTestBase
{
    private readonly IAbpApplicationManager _applicationManager;
    private readonly AbpOpenIddictTestData _testData;

    public AbpApplicationManager_Tests()
    {
        _applicationManager = ServiceProvider.GetRequiredService<IAbpApplicationManager>();
        _testData = ServiceProvider.GetRequiredService<AbpOpenIddictTestData>();
    }

    [Fact]
    public async Task Populate_Descriptor_With_Application_Test()
    {
        var app1 = (await _applicationManager.FindByClientIdAsync(_testData.App1ClientId)).As<OpenIddictApplicationModel>();

        var descriptor = new AbpApplicationDescriptor();
        await _applicationManager.PopulateAsync(descriptor, app1);

        app1.ClientUri.ShouldNotBeNull();
        app1.LogoUri.ShouldNotBeNull();

        descriptor.ClientUri.ShouldBe(app1.ClientUri);
        descriptor.LogoUri.ShouldBe(app1.LogoUri);
    }

    [Fact]
    public async Task Populate_Application_With_Descriptor_Test()
    {
        var app1 = (await _applicationManager.FindByClientIdAsync(_testData.App1ClientId)).As<OpenIddictApplicationModel>();

        var descriptor = new AbpApplicationDescriptor()
        {
            ClientUri = "https://new.com",
            LogoUri = "https://new.com/logo.png"
        };
        await _applicationManager.PopulateAsync(app1, descriptor);

        app1.ClientUri.ShouldBe(descriptor.ClientUri);
        app1.LogoUri.ShouldBe(descriptor.LogoUri);
    }
}
