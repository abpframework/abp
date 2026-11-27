using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.AgentTools.Sample;

/// <summary>Conventional controller under remote service "OtherSample", root path "other".</summary>
public interface IOtherSampleAppService : IApplicationService
{
    Task<string> GetGreetingAsync();

    /// <summary>Takes password-like values (top level and nested): they must never reach logs or call records.</summary>
    Task<string> RegisterAsync(RegisterSampleUserDto input, CancellationToken cancellationToken = default);
}

public class RegisterSampleUserDto
{
    public string UserName { get; set; } = default!;

    public string Password { get; set; } = default!;

    public SampleApiCredentialsDto? Api { get; set; }
}

public class SampleApiCredentialsDto
{
    public string Endpoint { get; set; } = default!;

    public string ApiKey { get; set; } = default!;
}

public class OtherSampleAppService : ApplicationService, IOtherSampleAppService
{
    public Task<string> GetGreetingAsync() => Task.FromResult("Hello from OtherSample");

    public Task<string> RegisterAsync(RegisterSampleUserDto input, CancellationToken cancellationToken = default)
    {
        return Task.FromResult("registered " + input.UserName);
    }
}
