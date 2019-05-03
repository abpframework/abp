namespace Volo.Abp.SolutionTemplating
{
    public class ProjectBuildResult
    {
        public byte[] ZipContent { get; }

        public string ProjectName { get; }

        public ProjectBuildResult(byte[] zipContent, string projectName)
        {
            ZipContent = zipContent;
            ProjectName = projectName;
        }
    }
}