namespace Volo.Abp.Cli.ProjectBuilding
{
    public class TemplateFile
    {
        public string Version { get; }

        public byte[] FileBytes { get; }

        public TemplateFile(byte[] fileBytes, string version)
        {
            FileBytes = fileBytes;
            Version = version;
        }
    }
}