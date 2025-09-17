using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Volo.Abp.Internal.Telemetry.Helpers;

static internal class AbpProjectMetadataReader
{
    private const string AbpPackageSearchPattern = "*.abppkg";
    private const string AbpSolutionSearchPattern = "*.abpsln";
    private const int MaxDepth = 10; 
    public static AbpProjectMetaData? ReadProjectMetadata(Assembly assembly)
    {
        var assemblyPath = assembly.Location;
        try
        {
            var projectDirectory = Path.GetDirectoryName(assemblyPath);
            if (projectDirectory == null)
            {
                return null;
            }

            var abpPackagePath = FindFileUpwards(projectDirectory, AbpPackageSearchPattern);

            if (abpPackagePath.IsNullOrEmpty())
            {
                return null;
            }
            
            var projectMetaData = ReadOrCreateMetadata(abpPackagePath);

            var abpSolutionPath = FindFileUpwards(projectDirectory, AbpSolutionSearchPattern);
            
            if (!abpSolutionPath.IsNullOrEmpty())
            {
                projectMetaData.AbpSlnPath = abpSolutionPath;
            }

            return projectMetaData;
        }
        catch
        {
            return null;
        }
    }

    private static AbpProjectMetaData ReadOrCreateMetadata(string packagePath)
    {
        
        var fileContent = File.ReadAllText(packagePath);
        var metadata = new AbpProjectMetaData();

        using var document = JsonDocument.Parse(fileContent);
        var root = document.RootElement;

        if (TryGetProjectId(root,out var projectId))
        {
            metadata.ProjectId = projectId;
        }
        else
        {
            metadata.ProjectId = Guid.NewGuid();
            WriteProjectIdToPackageFile(root, packagePath, metadata.ProjectId.Value);
        }

        if (root.TryGetProperty("role", out var roleElement) && 
            roleElement.ValueKind == JsonValueKind.String)
        {
            metadata.Role = roleElement.GetString()!;
        }
        
        return metadata;
    }

    private static void WriteProjectIdToPackageFile(JsonElement root, string packagePath, Guid projectId)
    {
        using var stream = new MemoryStream();
        using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true }))
        {
            writer.WriteStartObject();
                
            if (root.ValueKind == JsonValueKind.Object)
            {
                foreach (var property in root.EnumerateObject())
                {
                    if (property.Name != "projectId")
                    {
                        property.WriteTo(writer);
                    }
                }
            }
                
            writer.WriteString("projectId", projectId.ToString());
            writer.WriteEndObject();
        }

        var json = Encoding.UTF8.GetString(stream.ToArray());
        File.WriteAllText(packagePath, json);
    }

    private static string? FindFileUpwards(string startingDir, string searchPattern)
    {
        var currentDir = new DirectoryInfo(startingDir);
        var currentDepth = 0;

        while (currentDir != null && currentDepth < MaxDepth)
        {
            var file = currentDir.GetFiles(searchPattern).FirstOrDefault();
            if (file != null)
            {
                return file.FullName;
            }

            currentDir = currentDir.Parent;
            currentDepth++;
        }

        return null;
    }
    private static bool TryGetProjectId(JsonElement element, out Guid projectId)
    {
        if (element.TryGetProperty("projectId", out var projectIdElement) && 
            projectIdElement.ValueKind == JsonValueKind.String &&
            Guid.TryParse(projectIdElement.GetString(), out projectId))
        {
            return true;
        }

        projectId = Guid.Empty;
        return false;
    }
}