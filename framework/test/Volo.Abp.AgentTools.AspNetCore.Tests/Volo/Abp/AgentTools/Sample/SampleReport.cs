using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.AgentTools.Sample;

// Typical module HttpApi pattern: an app service interface, its implementation (not a conventional controller)
// and a hand-written controller implementing the same interface by forwarding to it.

public interface ISampleReportAppService : IApplicationService
{
    Task<ReportDto> GetAsync(Guid id);

    Task<ReportDto> GetAsync(string code);

    Task<ReportDto> CreateAsync(CreateReportDto input);

    /// <summary>Bound [FromForm] by the controller, so never a tool.</summary>
    Task<ReportDto> ImportAsync(ImportReportDto input);

    /// <summary>[DisableAgentTool] on the implementation class method only.</summary>
    Task<ReportDto> GetLatestAsync();

    /// <summary>[Authorize] only on the hand-written controller action: must not be bypassed in-process.</summary>
    Task DeleteAsync(Guid id);

    /// <summary>[AllowAnonymous] on the implementation class method only.</summary>
    Task<ReportDto> GetPublicAsync();
}

public class ReportDto
{
    public Guid Id { get; set; }

    public string Code { get; set; } = default!;
}

public class CreateReportDto
{
    [Required]
    [StringLength(32)]
    public string Code { get; set; } = default!;
}

public class ImportReportDto
{
    public string Code { get; set; } = default!;
}

[RemoteService(IsEnabled = false)]
public class SampleReportAppService : ApplicationService, ISampleReportAppService
{
    public Task<ReportDto> GetAsync(Guid id) => Task.FromResult(new ReportDto { Id = id, Code = "R-" + id.ToString("N")[..8] });

    [Description("Gets a report by its code.")] // on the implementation only
    public Task<ReportDto> GetAsync(string code) => Task.FromResult(new ReportDto { Id = Guid.Empty, Code = code });

    [AgentTool(Description = "Creates a report.")] // on the implementation only
    [Authorize(SamplePermissions.Reports.Create)]
    public Task<ReportDto> CreateAsync(CreateReportDto input) => Task.FromResult(new ReportDto { Id = GuidGenerator.Create(), Code = input.Code });

    public Task<ReportDto> ImportAsync(ImportReportDto input) => Task.FromResult(new ReportDto { Id = GuidGenerator.Create(), Code = input.Code });

    [DisableAgentTool]
    public Task<ReportDto> GetLatestAsync() => Task.FromResult(new ReportDto { Id = Guid.Empty, Code = "latest" });

    public Task DeleteAsync(Guid id) => Task.CompletedTask;

    [AllowAnonymous]
    public Task<ReportDto> GetPublicAsync() => Task.FromResult(new ReportDto { Id = Guid.Empty, Code = "public" });
}

[RemoteService(Name = SampleRemoteServices.AgentToolsSample)]
[Area(SampleRemoteServices.HandWrittenRootPath)]
[Route("api/agent-tools-sample/reports")]
public class SampleReportController : AbpControllerBase, ISampleReportAppService
{
    private readonly ISampleReportAppService _reports;

    public SampleReportController(ISampleReportAppService reports)
    {
        _reports = reports;
    }

    [HttpGet("{id}")]
    public Task<ReportDto> GetAsync(Guid id) => _reports.GetAsync(id);

    [HttpGet("by-code/{code}")]
    public Task<ReportDto> GetAsync(string code) => _reports.GetAsync(code);

    [HttpPost]
    public Task<ReportDto> CreateAsync(CreateReportDto input) => _reports.CreateAsync(input);

    [HttpPost("import")]
    public Task<ReportDto> ImportAsync([FromForm] ImportReportDto input) => _reports.ImportAsync(input);

    [HttpGet("latest")]
    public Task<ReportDto> GetLatestAsync() => _reports.GetLatestAsync();

    [HttpDelete("{id}")]
    [Authorize(SamplePermissions.Reports.Delete)]
    public Task DeleteAsync(Guid id) => _reports.DeleteAsync(id);

    [HttpGet("public")]
    public Task<ReportDto> GetPublicAsync() => _reports.GetPublicAsync();
}

/// <summary>Hand-written controller without a service interface: skipped by the catalogue.</summary>
[RemoteService(Name = SampleRemoteServices.AgentToolsSample)]
[Area(SampleRemoteServices.HandWrittenRootPath)]
[Route("api/agent-tools-sample/ping")]
public class SamplePingController : AbpControllerBase
{
    [HttpGet]
    public string Get() => "pong";
}

/// <summary>An interface nobody registers in DI: its hand-written controller is skipped ("service interface not registered").</summary>
public interface ISampleOrphanAppService : IApplicationService
{
    Task<string> GetAsync();
}

[RemoteService(Name = SampleRemoteServices.AgentToolsSample)]
[Area(SampleRemoteServices.HandWrittenRootPath)]
[Route("api/agent-tools-sample/orphan")]
public class SampleOrphanController : AbpControllerBase, ISampleOrphanAppService
{
    [HttpGet]
    public Task<string> GetAsync() => Task.FromResult("orphan");
}
