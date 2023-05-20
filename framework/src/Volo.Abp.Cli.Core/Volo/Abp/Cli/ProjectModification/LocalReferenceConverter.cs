using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectModification;

public class LocalReferenceConverter : ITransientDependency
{
    
    public ILogger<LocalReferenceConverter> Logger { get; set; }
    
    public async Task ConvertAsync(
        [NotNull] string directory,
        [NotNull] List<string> localPaths)
    {
        Check.NotNull(directory, nameof(directory));
        Check.NotNull(localPaths, nameof(localPaths));

        var localProjects = GetLocalProjects(localPaths);
        var targetProjects = Directory.GetFiles(directory, "*.csproj", SearchOption.AllDirectories);

        Logger.LogInformation($"Converting projects to local reference.");
        
        foreach (var targetProject in targetProjects)
        {
            Logger.LogInformation($"Converting to local reference: {targetProject}");
            
            await ConvertProjectToLocalReferences(targetProject, localProjects);
        }
        
        Logger.LogInformation($"Converted {targetProjects.Length} projects to local references.");
    }

    private async Task ConvertProjectToLocalReferences(string targetProject, List<string> localProjects)
    {
        var xmlDocument = new XmlDocument() { PreserveWhitespace = true };
        xmlDocument.Load(GenerateStreamFromString(File.ReadAllText(targetProject)));
        
        var matchedNodes = xmlDocument.SelectNodes($"/Project/ItemGroup/PackageReference[@Include]");

        if (matchedNodes == null || matchedNodes.Count == 0)
        {
            return;
        }
        
        foreach (XmlNode matchedNode in matchedNodes)
        {
            var packageName = matchedNode!.Attributes!["Include"].Value;

            var localProject = localProjects.Find(x =>
                x.EndsWith($"\\{packageName}.csproj") ||
                x.EndsWith($"/{packageName}.csproj")
            );

            if (localProject == null)
            {
                continue;
            }
            
            var parentNode = matchedNode.ParentNode;
            parentNode!.RemoveChild(matchedNode);

            var newNode = xmlDocument.CreateElement("ProjectReference");
            var includeAttr = xmlDocument.CreateAttribute("Include");
            includeAttr.Value = CalculateRelativePath(targetProject, localProject);
            newNode.Attributes.Append(includeAttr);
            parentNode.AppendChild(newNode);
        }
        
        File.WriteAllText(targetProject, XDocument.Parse(xmlDocument.OuterXml).ToString());
    }
    
    private string CalculateRelativePath(string targetProject, string localProject)
    {
        return new Uri(targetProject).MakeRelativeUri(new Uri(localProject)).ToString();
    }

    private List<string> GetLocalProjects(List<string> localPaths)
    {
        var list = new List<string>();

        foreach (var localPath in localPaths)
        {
            if (!Directory.Exists(localPath))
            {
                continue;
            }
            
            list.AddRange(Directory.GetFiles(localPath, "*.csproj", SearchOption.AllDirectories));
        }

        return list;
    }

    private MemoryStream GenerateStreamFromString(string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
}
