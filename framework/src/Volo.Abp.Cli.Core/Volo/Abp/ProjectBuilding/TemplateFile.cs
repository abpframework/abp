namespace Volo.Abp.ProjectBuilding
{
    public class TemplateFile
    {
        public byte[] FileBytes { get; }

        public TemplateFile(byte[] fileBytes)
        {
            FileBytes = fileBytes;
        }
    }
}