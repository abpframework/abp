using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Cli.ProjectBuilding.Building;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.App;

public class AppTemplateRemoveMongodbCollectionFixtureStep : ProjectBuildPipelineStep
{
    public override void Execute(ProjectBuildContext context)
    {
        //MyCompanyName.MyProjectName.Application.Tests
        RemoveFile(context, "/aspnet-core/test/MyCompanyName.MyProjectName.Application.Tests/MyProjectNameApplicationCollection.cs");
        RemoveKeyword(context,
            "/aspnet-core/test/MyCompanyName.MyProjectName.Application.Tests/Samples/SampleAppServiceTests.cs",
            "[Collection(MyProjectNameTestConsts.CollectionDefinitionName)]");

        //MyCompanyName.MyProjectName.Domain.Tests
        RemoveFile(context, "/aspnet-core/test/MyCompanyName.MyProjectName.Domain.Tests/MyProjectNameDomainCollection.cs");
        RemoveKeyword(context,
            "/aspnet-core/test/MyCompanyName.MyProjectName.Domain.Tests/Samples/SampleDomainTests.cs",
            "[Collection(MyProjectNameTestConsts.CollectionDefinitionName)]");

        //MyCompanyName.MyProjectName.EntityFrameworkCore.Tests
        RemoveFile(context, "/aspnet-core/test/MyCompanyName.MyProjectName.EntityFrameworkCore.Tests/EntityFrameworkCore/MyProjectNameEntityFrameworkCoreCollection.cs");
        RemoveFile(context, "/aspnet-core/test/MyCompanyName.MyProjectName.EntityFrameworkCore.Tests/EntityFrameworkCore/MyProjectNameEntityFrameworkCoreCollectionFixtureBase.cs");
        RemoveFile(context, "/aspnet-core/test/MyCompanyName.MyProjectName.EntityFrameworkCore.Tests/EntityFrameworkCore/MyProjectNameEntityFrameworkCoreFixture.cs");
        RemoveKeyword(context,
            "/aspnet-core/test/MyCompanyName.MyProjectName.EntityFrameworkCore.Tests/EntityFrameworkCore/Samples/SampleRepositoryTests.cs",
            "[Collection(MyProjectNameTestConsts.CollectionDefinitionName)]");

        //MyCompanyName.MyProjectName.Web.Tests
        RemoveFile(context, "/aspnet-core/test/MyCompanyName.MyProjectName.Web.Tests/MyProjectNameWebCollection.cs");
        RemoveKeyword(context,
            "/aspnet-core/test/MyCompanyName.MyProjectName.Web.Tests/Pages/Index_Tests.cs",
            "[Collection(MyProjectNameTestConsts.CollectionDefinitionName)]");

        //MyCompanyName.MyProjectName.TestBase
        RemoveFile(context, "/aspnet-core/test/MyCompanyName.MyProjectName.TestBase/MyProjectNameTestConsts.cs");
    }

    private static void RemoveFile(ProjectBuildContext context, string targetModuleFilePath)
    {
        var file = context.Files.FirstOrDefault(x => x.Name == targetModuleFilePath);
        if (file != null)
        {
            context.Files.Remove(file);
        }
    }

    private static void RemoveKeyword(ProjectBuildContext context, string targetModuleFilePath, string keyword)
    {
        var file = context.GetFile(targetModuleFilePath);

        file.NormalizeLineEndings();

        var lines = file.GetLines();
        var newLines = new List<string>();

        for (var i = 0; i < lines.Length; i++)
        {
            if (!lines[i].Contains(keyword))
            {
                newLines.Add(lines[i]);
            }
        }

        file.SetLines(newLines);
    }
}
