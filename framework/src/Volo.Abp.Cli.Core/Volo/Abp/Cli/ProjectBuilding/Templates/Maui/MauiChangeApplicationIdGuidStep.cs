using System;
using System.Linq;
using System.Xml;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.Utils;

namespace Volo.Abp.Cli.ProjectBuilding.Templates.Maui;

public class MauiChangeApplicationIdGuidStep: ProjectBuildPipelineStep
{
    public override void Execute(ProjectBuildContext context)
    {
        var projectFile = context.Files.FirstOrDefault(f => f.Name.EndsWith("MyCompanyName.MyProjectName.csproj") || f.Name.EndsWith("MyCompanyName.MyProjectName.Maui.csproj"));

        if (projectFile == null)
        {
            return;
        }

        using (var stream = StreamHelper.GenerateStreamFromString(projectFile.Content))
        {
            var doc = new XmlDocument { PreserveWhitespace = true };
            doc.Load(stream);
                
            var node = doc.SelectSingleNode("/Project/PropertyGroup/ApplicationIdGuid");
            if (node != null)
            {
                node.InnerText = Guid.NewGuid().ToString();
            }
                
            projectFile.SetContent(doc.OuterXml);
        }
    }
}