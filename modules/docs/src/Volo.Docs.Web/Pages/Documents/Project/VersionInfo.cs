namespace Volo.Docs.Pages.Documents.Project
{
    public class VersionInfo
    {
        public string DisplayText { get; set; }

        public string Version { get; set; }

        public bool IsSelected { get; set; }

        public VersionInfo(string displayText, string version, bool isSelected = false)
        {
            DisplayText = displayText;
            Version = version;
            IsSelected = isSelected;
        }
    }
}