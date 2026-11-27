using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AgentTools.Sample.Binding;

// Remote service "BindingSample", own namespace so the other samples' tool lists stay as they are: parameters from services and
// headers next to ordinary ones.

public class BindingSampleRules : ITransientDependency
{
    public decimal Discount { get; set; } = 0.1m;
}

public class BindingSampleInput
{
    public string? Name { get; set; }

    [FromHeader(Name = "X-Owner")]
    public string? Owner { get; set; }
}

/// <summary>Conventional controller, root path "binding".</summary>
public interface IBindingSampleAppService : IApplicationService
{
    Task<string> GetAsync(Guid id, CancellationToken cancellationToken = default);

    Task<string> GetPageAsync(PagedAndSortedResultRequestDto input);

    /// <summary>The members of a request body come from the body, also those with [FromHeader].</summary>
    Task<string> CreateAsync(BindingSampleInput input);

    Task<string> GetFromServicesAsync(string name, BindingSampleRules rules);

    Task<string> GetFromKeyedServicesAsync(BindingSampleRules rules);

    Task<string> GetFromKeyedServicesOnTheInterfaceAsync([FromKeyedServices("rules")] BindingSampleRules rules);

    Task<string> GetFromHeaderAsync(string caller);

    Task<string> GetFromHeaderOnTheInterfaceAsync([FromHeader] string caller);

    Task<string> GetWithHeaderMemberAsync(BindingSampleInput input);
}

public class BindingSampleAppService : ApplicationService, IBindingSampleAppService
{
    public const string RemoteServiceName = "BindingSample";
    public const string RootPath = "binding";

    public Task<string> GetAsync(Guid id, CancellationToken cancellationToken = default) => Task.FromResult(id.ToString());

    public Task<string> GetPageAsync(PagedAndSortedResultRequestDto input) => Task.FromResult("");

    public Task<string> CreateAsync(BindingSampleInput input) => Task.FromResult("");

    public Task<string> GetFromServicesAsync(string name, [FromServices] BindingSampleRules rules) => Task.FromResult("");

    public Task<string> GetFromKeyedServicesAsync([FromKeyedServices("rules")] BindingSampleRules rules) => Task.FromResult("");

    public Task<string> GetFromKeyedServicesOnTheInterfaceAsync(BindingSampleRules rules) => Task.FromResult("");

    public Task<string> GetFromHeaderAsync([FromHeader] string caller) => Task.FromResult("");

    public Task<string> GetFromHeaderOnTheInterfaceAsync(string caller) => Task.FromResult("");

    public Task<string> GetWithHeaderMemberAsync(BindingSampleInput input) => Task.FromResult("");
}

/// <summary>Behind a hand-written controller.</summary>
public interface IBindingSampleReportAppService : IApplicationService
{
    Task<string> GetAsync(Guid id, string filter);

    Task<string> CreateAsync(BindingSampleInput input);

    Task<string> GetFromServicesAsync(BindingSampleRules rules);

    Task<string> GetFromInferredServicesAsync(BindingSampleRules rules);

    Task<string> GetFromServicesOnTheImplementationAsync(BindingSampleRules rules);
}

[RemoteService(IsEnabled = false)]
public class BindingSampleReportAppService : ApplicationService, IBindingSampleReportAppService
{
    public Task<string> GetAsync(Guid id, string filter) => Task.FromResult("");

    public Task<string> CreateAsync(BindingSampleInput input) => Task.FromResult("");

    public Task<string> GetFromServicesAsync(BindingSampleRules rules) => Task.FromResult("");

    public Task<string> GetFromInferredServicesAsync(BindingSampleRules rules) => Task.FromResult("");

    public Task<string> GetFromServicesOnTheImplementationAsync([FromServices] BindingSampleRules rules) => Task.FromResult("");
}

[ApiController] // infers [FromServices] for a parameter of a registered type
[RemoteService(Name = BindingSampleAppService.RemoteServiceName)]
[Area("bindingReports")]
[Route("api/binding-sample/reports")]
public class BindingSampleReportController : AbpControllerBase, IBindingSampleReportAppService
{
    private readonly IBindingSampleReportAppService _reports;

    /// <summary>Not an argument of the tools.</summary>
    [FromHeader(Name = "X-Caller")]
    public string? Caller { get; set; }

    public BindingSampleReportController(IBindingSampleReportAppService reports)
    {
        _reports = reports;
    }

    [HttpGet("{id}")]
    public Task<string> GetAsync([FromRoute] Guid id, [FromQuery] string filter) => _reports.GetAsync(id, filter);

    [HttpPost]
    public Task<string> CreateAsync([FromBody] BindingSampleInput input) => _reports.CreateAsync(input);

    [HttpGet("from-services")]
    public Task<string> GetFromServicesAsync([FromServices] BindingSampleRules rules) => _reports.GetFromServicesAsync(rules);

    [HttpGet("from-inferred-services")]
    public Task<string> GetFromInferredServicesAsync(BindingSampleRules rules) => _reports.GetFromInferredServicesAsync(rules);

    [HttpPost("from-services-on-the-implementation")]
    public Task<string> GetFromServicesOnTheImplementationAsync([FromBody] BindingSampleRules rules) =>
        _reports.GetFromServicesOnTheImplementationAsync(rules);
}
