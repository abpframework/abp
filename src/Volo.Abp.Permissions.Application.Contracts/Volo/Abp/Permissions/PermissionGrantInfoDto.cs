namespace Volo.Abp.Permissions
{
    public class PermissionGrantInfoDto
    {
        public string Name { get; set; }

        public string ParentName { get; set; }

        public bool IsGranted { get; set; }

        public string ProviderName { get; set; }

        public string ProviderKey { get; set; }
    }
}