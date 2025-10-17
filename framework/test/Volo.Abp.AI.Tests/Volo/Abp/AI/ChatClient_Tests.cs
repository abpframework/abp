using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using Shouldly;
using Volo.Abp.AI.Mocks;
using Volo.Abp.AI.Tests.Workspaces;
using Volo.Abp.AutoMapper;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.AI.Tests;

public class ChatClient_Tests : AbpIntegratedTest<AbpAITestModule>
{
    [Fact]
    public void Should_Resolve_ChatClient_For_Workspace()
    {
        // Arrange & Act
        var chatClient = GetRequiredService<IChatClient<WordCounter>>();

        // Assert
        chatClient.ShouldNotBeNull();
        chatClient.ShouldNotBeOfType<MockDefaultChatClient>();
    }

    [Fact]
    public void Should_Resolve_Keyed_ChatClient_For_Workspace()
    {
        // Arrange
        var workspaceName = WorkspaceNameAttribute.GetWorkspaceName<WordCounter>();
        var serviceName = AbpAIWorkspaceOptions.GetChatClientServiceKeyName(workspaceName);

        // Act
        var chatClient = GetRequiredKeyedService<IChatClient>(
            serviceName
        );

        // Assert
        chatClient.ShouldNotBeNull();
    }

    [Fact]
    public void Should_Resolve_Default_ChatClient()
    {
        // Arrange & Act
        var chatClient = GetRequiredService<IChatClient>();
        
        // Assert
        chatClient.ShouldNotBeNull();
        chatClient.ShouldBeOfType<MockDefaultChatClient>();
    }

    [Fact]
    public async Task Should_Get_Response_For_Workspace()
    {
        // Arrange
        var chatClient = GetRequiredService<IChatClient<WordCounter>>();

        // Act
        var response = await chatClient.GetResponseAsync(new[]
        {
            new ChatMessage(ChatRole.User, "Hello, how are you?")
        });

        // Assert
        response.ShouldNotBeNull();
        response.Messages.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task Should_Get_Streaming_Response_For_Workspace()
    {
        // Arrange
        var chatClient = GetRequiredService<IChatClient<WordCounter>>();
        var messagesInput = new[]
        {
            new ChatMessage(ChatRole.User, "Hello, how are you?")
        };

        // Act
        var responseParts = 0;
        await foreach (var response in chatClient.GetStreamingResponseAsync(messagesInput))
        {
            responseParts++;
        }

        // Assert
        responseParts.ShouldBe(MockChatClient.StreamingResponseParts);
    }
}