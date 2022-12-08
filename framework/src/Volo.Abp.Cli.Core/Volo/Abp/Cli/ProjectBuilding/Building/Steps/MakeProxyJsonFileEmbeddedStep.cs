using System.Linq;
using System.Xml.Linq;
using Volo.Abp.Cli.Utils;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class MakeProxyJsonFileEmbeddedStep : ProjectBuildPipelineStep
{
    public override void Execute(ProjectBuildContext context)
    {
        foreach (var file in context.Files.Where(x => x.Name.EndsWith(".HttpApi.Client.csproj")))
        {
            using (var stream = StreamHelper.GenerateStreamFromString(file.Content))
            {
                var doc = XDocument.Load(stream);

                if (doc.Root == null)
                {
                    continue;
                }

                var itemGroupNode =
                    new XElement("ItemGroup",
                        new XElement("EmbeddedResource",
                            new XAttribute("Include", @"**\*generate-proxy.json")
                        ),
                        new XElement("Content",
                            new XAttribute("Remove", @"**\*generate-proxy.json")
                        )
                    );

                doc.Root.Add(itemGroupNode);

                file.SetContent(doc.ToString());
            }
        }
    }
}
