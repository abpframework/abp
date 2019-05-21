namespace Volo.Abp.Emailing.Templates.VirtualFiles
{
    public static class EmailTemplateDefinitionExtensions
    {
        public static EmailTemplateDefinition AddTemplateVirtualFile(
            this EmailTemplateDefinition emailTemplateDefinition, string path)
        {
            emailTemplateDefinition.Contributors.Add(new VirtualFileEmailTemplateContributor(path));
            return emailTemplateDefinition;
        }
    }
}