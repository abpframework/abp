using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using Shouldly;
using Volo.Abp.AI.Mocks;
using Volo.Abp.AI.Tests.Workspaces;
using Volo.Abp.AutoMapper;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.AI;
public class KernelAccessor_Tests : AbpIntegratedTest<AbpAITestModule>
{
    [Fact]
    public void Should_Resolve_DefaultKernelAccessor()
    {
        // Arrange & Act
        var kernelAccessor = GetRequiredService<IKernelAccessor>();
        // Assert
        kernelAccessor.ShouldNotBeNull();
        kernelAccessor.Kernel.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Get_Response_From_DefaultKernel()
    {
        // Arrange
        var kernelAccessor = GetRequiredService<IKernelAccessor>();
        var kernel = kernelAccessor.Kernel;
        // Act
        var result = await kernel.GetRequiredService<IChatClient>()
            .GetResponseAsync("Hello, World!");
        // Assert
        result.ShouldNotBeNull();
        result.RawRepresentation.ShouldBe(MockChatClient.MockResponse);
    }

    [Fact]
    public void Should_Resolve_KernelAccessor_For_Workspace()
    {
        // Arrange & Act
        var kernelAccessor = GetRequiredService<IKernelAccessor<WordCounter>>();
        // Assert
        kernelAccessor.ShouldNotBeNull();
        kernelAccessor.Kernel.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Get_Response_From_Kernel_For_Workspace()
    {
        // Arrange
        var kernelAccessor = GetRequiredService<IKernelAccessor<WordCounter>>();
        var kernel = kernelAccessor.Kernel;

        // Act
        var result = await kernel.GetRequiredService<IChatClient>()
            .GetResponseAsync("Hello, World!");

        // Assert
        result.ShouldNotBeNull();
        result.RawRepresentation.ShouldBe(MockChatClient.MockResponse);
    }
}
