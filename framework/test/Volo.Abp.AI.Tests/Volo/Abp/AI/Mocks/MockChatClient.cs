using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.AI;

namespace Volo.Abp.AI.Mocks;

public class MockChatClient : IChatClient
{
    public const int StreamingResponseParts = 4;

    public const string MockResponse = "This is a mock response.";
    public void Dispose()
    {

    }

    public Task<ChatResponse> GetResponseAsync(
        IEnumerable<ChatMessage> messages,
        ChatOptions options = null,
        CancellationToken cancellationToken = default)
    {
        var responseMessages = messages.ToList();
        responseMessages.Add(new ChatMessage(ChatRole.Assistant, MockResponse));
        return Task.FromResult(new ChatResponse
        {
            Messages = responseMessages,
            RawRepresentation = MockResponse
        });
    }

    public object GetService(Type serviceType, object serviceKey = null)
    {
        return null;
    }

    public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(
        IEnumerable<ChatMessage> messages,
        ChatOptions options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        for (var i = 0; i < StreamingResponseParts; i++)
        {
            await Task.Delay(25, cancellationToken);

            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            yield return new ChatResponseUpdate
            {
                Role = ChatRole.Assistant,
                RawRepresentation = MockResponse + " " + (i + 1),
            };
        }
    }
}
