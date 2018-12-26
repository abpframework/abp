namespace Volo.Utils.SolutionTemplating.Building
{
    public class ProjectBuildRequest
    {
        public SolutionName SolutionName { get; }

        public DatabaseProvider DatabaseProvider { get; }

        public string Version { get; }

        public bool ReplaceLocalReferencesToNuget { get; set; }

        public ProjectBuildRequest(
            SolutionName solutionName, 
            DatabaseProvider databaseProvider, 
            string version)
        {
            SolutionName = solutionName;
            DatabaseProvider = databaseProvider;
            Version = version;
        }
    }
}