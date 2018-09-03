namespace Volo.Abp.Emailing.Templates.Virtual
{
    public static class EmailTemplateDefinitionExtensions
    {
        public static EmailTemplateDefinition SetVirtualFilePath(this EmailTemplateDefinition emailTemplateDefinition, string path)
        {
            emailTemplateDefinition[VirtualFileEmailTemplateProviderContributor.VirtualFilePathKey] = path;
            return emailTemplateDefinition;
        }

        public static string GetVirtualFilePathOrNull(this EmailTemplateDefinition emailTemplateDefinition)
        {
            return emailTemplateDefinition[VirtualFileEmailTemplateProviderContributor.VirtualFilePathKey] as string;
        }
    }
}