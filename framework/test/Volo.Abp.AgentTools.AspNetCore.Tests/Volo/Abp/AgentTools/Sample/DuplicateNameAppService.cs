using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.AgentTools.Sample;

/// <summary>
/// Remote service "DuplicateSample": its tool names collide with Book_GetList and SampleReport_Get_2 (an overload), with and
/// without the same case, and with each other, so including it must fail fast. Never include it in a host's options.
/// </summary>
public interface IDuplicateNameAppService : IApplicationService
{
    [AgentTool(Name = "Book_GetList")]
    Task<int> GetBookCountAsync();

    [AgentTool(Name = "book_getlist")]
    Task<int> GetLowerCaseCountAsync();

    [AgentTool(Name = "SAMPLEREPORT_GET_2")]
    Task<int> GetReportCountAsync();
}

public class DuplicateNameAppService : ApplicationService, IDuplicateNameAppService
{
    public Task<int> GetBookCountAsync() => Task.FromResult(0);

    public Task<int> GetLowerCaseCountAsync() => Task.FromResult(0);

    public Task<int> GetReportCountAsync() => Task.FromResult(0);
}
