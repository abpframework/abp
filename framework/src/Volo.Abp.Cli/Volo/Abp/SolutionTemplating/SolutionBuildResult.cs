namespace Volo.Abp.SolutionTemplating
{
    public class SolutionBuildResult
    {
        public byte[] ZipContent { get; }

        public string ProjectName { get; }

        public SolutionBuildResult(byte[] zipContent, string projectName)
        {
            ZipContent = zipContent;
            ProjectName = projectName;
        }
    }
}