namespace Volo.Abp.Cli.ProjectBuilding
{
    public class TemplateFile
    {
        public string Version { get; }

        public string LatestVersion { get; }

        public byte[] FileBytes { get; }

        public TemplateFile(byte[] fileBytes, string version, string latestVersion)
        {
            FileBytes = fileBytes;
            Version = version;
            LatestVersion = latestVersion;
        }
    }
}