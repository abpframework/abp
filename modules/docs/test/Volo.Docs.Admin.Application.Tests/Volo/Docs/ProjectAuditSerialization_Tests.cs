using System.Collections.Generic;
using Shouldly;
using Volo.Abp.Auditing;
using Volo.Abp.Json;
using Volo.Docs.Admin.Projects;
using Xunit;

namespace Volo.Docs;

public class ProjectAuditSerialization_Tests : DocsAdminApplicationTestBase
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Should_Exclude_Project_Credentials_From_Audit_Parameters(bool update)
    {
        const string token = "synthetic-project-credential";
        var properties = new Dictionary<string, object> { ["GitHubAccessToken"] = token };
        object input = update
            ? new UpdateProjectDto { Name = "Docs project", ExtraProperties = properties }
            : new CreateProjectDto { Name = "Docs project", ExtraProperties = properties };

        var auditJson = GetRequiredService<IAuditSerializer>().Serialize(new Dictionary<string, object> { ["input"] = input });
        var requestJson = GetRequiredService<IJsonSerializer>().Serialize(input);

        auditJson.ShouldNotContain(token);
        auditJson.ShouldContain("Docs project");
        requestJson.ShouldContain(token);
    }
}
