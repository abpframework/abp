namespace Volo.Abp.Emailing.Templates.VirtualFiles
{
    public static class EmailTemplateDefinitionExtensions
    {
        public static EmailTemplateDefinition AddTemplateVirtualFile(
            this EmailTemplateDefinition emailTemplateDefinition, string path)
        {
            emailTemplateDefinition.Contributors.Add(new SingleVirtualFileEmailTemplateContributor(path));
            return emailTemplateDefinition;
        }

        public static EmailTemplateDefinition AddTemplateVirtualFiles(
            this EmailTemplateDefinition emailTemplateDefinition, string path)
        {
            emailTemplateDefinition.Contributors.Add(new MultipleVirtualFilesEmailTemplateContributor(path));
            return emailTemplateDefinition;
        }
    }
}