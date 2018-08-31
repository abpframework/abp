namespace Volo.Abp.Emailing.Templates.Virtual
{
    public static class EmailTemplateDefinitionExtensions
    {
        public static EmailTemplateDefinition SetVirtualFilePath(this EmailTemplateDefinition emailTemplateDefinition, string path)
        {
            emailTemplateDefinition[VirtualFileEmailTemplateProvider.VirtualFilePathKey] = path;
            return emailTemplateDefinition;
        }

        public static string GetVirtualFilePathOrNull(this EmailTemplateDefinition emailTemplateDefinition)
        {
            return emailTemplateDefinition[VirtualFileEmailTemplateProvider.VirtualFilePathKey] as string;
        }
    }
}