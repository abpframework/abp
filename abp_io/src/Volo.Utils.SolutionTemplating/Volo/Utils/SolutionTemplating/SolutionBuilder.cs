using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Utils.SolutionTemplating.Building;

namespace Volo.Utils.SolutionTemplating
{
    public class SolutionBuilder : ITransientDependency
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IRepository<DownloadInfo> _downloadRepository;

        public SolutionBuilder(
            IHostingEnvironment hostingEnvironment,
            IRepository<DownloadInfo> downloadRepository)
        {
            _hostingEnvironment = hostingEnvironment;
            _downloadRepository = downloadRepository;
        }

        public async Task<SolutionBuildResult> BuildAsync(
            TemplateInfo template,
            string companyAndProjectName, 
            DatabaseProvider databaseProvider,
            string version,
            bool replaceLocalReferencesToNuget)
        {
            var stopwatch = Stopwatch.StartNew();

            var templateRootPath = Path.Combine(_hostingEnvironment.ContentRootPath, "TemplateFiles"); //TODO: To another folder that is not in the deployment folder!
            var solutionName = SolutionName.Parse(companyAndProjectName);

            var context = new ProjectBuildContext(
                template,
                new ProjectBuildRequest(
                    solutionName,
                    databaseProvider,
                    version
                ),
                templateRootPath
            );

            ProjectBuildPipelineBuilder.Build(context).Execute(context);

            stopwatch.Stop();

            await _downloadRepository.InsertAsync(
                new DownloadInfo(
                    solutionName.FullName,
                    template.Name,
                    databaseProvider,
                    context.Template.Version,
                    Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds)
                )
            );

            return new SolutionBuildResult(context.Result.ZipContent, solutionName.ProjectName);
        }
    }
}
