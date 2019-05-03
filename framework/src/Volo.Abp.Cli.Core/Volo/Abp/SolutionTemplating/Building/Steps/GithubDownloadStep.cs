using System;
using System.IO;
using Volo.Abp.SolutionTemplating.Github;

namespace Volo.Abp.SolutionTemplating.Building.Steps
{
    public class GithubDownloadStep : ProjectBuildPipelineStep
    {
        public override void Execute(ProjectBuildContext context)
        {
            var githubManager = new GithubManager();

            context.Template.Version = githubManager.GetVersion(context);

            context.Template.FilePath = Path.Combine(
                context.TemplatesFolder,
                $"{context.Template.Name}-{context.Template.Version}.zip"
            );

            context.Template.RootPathInZipFile = context.Template.GithubRepository.RepositoryName + "-" + context.Template.Version.RemovePreFix("v") + context.Template.RootPathInZipFile;

            try
            {
                githubManager.DownloadIfNotExist(context);
            }
            catch (Exception e)
            {
                throw e; //TODO: !!!
                //var existingVersion = githubManager.GetExistingVersion(context);
            }
        }
    }
}