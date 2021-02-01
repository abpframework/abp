namespace Volo.CmsKit.Admin.Tags
{
    public class TagDefinitionDto
    {
        public TagDefinitionDto()
        {
        }
        public TagDefinitionDto(string entityType, string displayName)
        {
            EntityType = entityType;
            DisplayName = displayName;
        }

        public string EntityType { get; set; }

        public string DisplayName { get; set; }
    }
}
