namespace Volo.Docs.Models
{
    public class VersionInfoViewModel
    {
        public string DisplayText { get; set; }

        public string Version { get; set; }

        public bool IsSelected { get; set; }

        public bool IsPreview { get; set; }

        public VersionInfoViewModel(string displayText, string version, bool isSelected = false)
        {
            DisplayText = displayText;
            Version = version;
            IsSelected = isSelected;
        }
    }
}
