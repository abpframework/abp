using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps
{
    public class RemoveDependencyFromPackageJsonFileStep : ProjectBuildPipelineStep
    {
        private readonly string _packageJsonFilePath;
        private readonly string _packageName;

        public RemoveDependencyFromPackageJsonFileStep(string packageJsonFilePath, string packageName)
        {
            _packageJsonFilePath = packageJsonFilePath;
            _packageName = packageName;
        }

        public override void Execute(ProjectBuildContext context)
        {
            var packageJsonFile = context.Files.FirstOrDefault(f => f.Name == _packageJsonFilePath);

            if (packageJsonFile == null)
            {
                return;
            }

            var packageJsonObject = JObject.Parse(packageJsonFile.Content);
            var dependenciesObject = (JObject) packageJsonObject["dependencies"];

            dependenciesObject?.Remove(_packageName);

            packageJsonFile.SetContent(packageJsonObject.ToString(Formatting.Indented));
        }
    }
}
