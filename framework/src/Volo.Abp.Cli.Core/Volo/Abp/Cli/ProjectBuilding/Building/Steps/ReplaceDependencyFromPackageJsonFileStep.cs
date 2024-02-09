using System.Linq;
using System.Text.RegularExpressions;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class ReplaceDependencyFromPackageJsonFileStep : ProjectBuildPipelineStep
{
    private readonly string _packageJsonFilePath;
    private readonly string _packageName;
    private readonly string _newPackageName;
    private readonly string _newPackageVersion;

    public ReplaceDependencyFromPackageJsonFileStep(string packageJsonFilePath, string packageName, string newPackageName, string newPackageVersion)
    {
        _packageJsonFilePath = packageJsonFilePath;
        _packageName = packageName;
        _newPackageName = newPackageName;
        _newPackageVersion = newPackageVersion;
    }

    public override void Execute(ProjectBuildContext context)
    {
        var packageJsonFile = context.Files.FirstOrDefault(f => f.Name.Contains(_packageJsonFilePath) && !f.Name.Contains("node_modules"));
        if (packageJsonFile == null)
        {
            return;
        }

        var lines = packageJsonFile.GetLines();
        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains(_packageName))
            {
                if (_newPackageVersion == null)
                {
                    lines[i] = lines[i].Replace(_packageName, _newPackageName);
                }
                else
                {
                    var space = Regex.Match(lines[i], @"\s+").Value;
                    var hasComma = lines[i].Contains(",") ? "," : "";
                    var caret = lines[i].Contains("^");
                    var tilde = lines[i].Contains("~");
                    var prefix = caret ? "^" : tilde ? "~" : "";
                    lines[i] = $"{space}\"{_newPackageName}\": \"{prefix}{_newPackageVersion}\"{hasComma}";
                }
            }
        }
        packageJsonFile.SetLines(lines);
    }
}
