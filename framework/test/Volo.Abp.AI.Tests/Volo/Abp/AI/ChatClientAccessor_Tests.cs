using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using Volo.Abp.AI.Tests.Workspaces;
using Volo.Abp.AutoMapper;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.AI;
public class ChatClientAccessor_Tests : AbpIntegratedTest<AbpAITestModule>
{
    [Fact]
    public void Should_Resolve_DefaultChatClientAccessor()
    {
        // Arrange & Act
        var chatClientAccessor = GetRequiredService<IChatClientAccessor>();
        // Assert
        chatClientAccessor.ShouldNotBeNull();
        chatClientAccessor.ChatClient.ShouldNotBeNull();
    }

    [Fact]
    public void Should_Resolve_ChatClientAccessor_For_Workspace()
    {
        // Arrange & Act
        var chatClientAccessor = GetRequiredService<IChatClientAccessor<WordCounter>>();
        // Assert
        chatClientAccessor.ShouldNotBeNull();
        chatClientAccessor.ChatClient.ShouldNotBeNull();
    }

    [Fact]
    public void Should_Resolve_Default_ChatClient_From_NonConfigured_Workspace_Accessor()
    {
        // Arrange & Act
        var chatClientAccessor = GetRequiredService<IChatClientAccessor<NonConfiguredWorkspace>>();

        // Assert
        chatClientAccessor.ShouldNotBeNull();
        chatClientAccessor.ChatClient.ShouldNotBeNull();
    }

    [Fact]
    public void Should_Resolve_Default_ChatClient_For_NonConfigured_Workspace()
    {
        // Arrange & Act
        var chatClient = GetRequiredService<IChatClient<NonConfiguredWorkspace>>();

        // Assert
        chatClient.ShouldNotBeNull();
    }

    public class NonConfiguredWorkspace
    {
    }
}