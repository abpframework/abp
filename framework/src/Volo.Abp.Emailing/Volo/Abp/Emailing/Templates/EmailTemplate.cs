namespace Volo.Abp.Emailing.Templates
{
    public class EmailTemplate
    {
        public string Content { get; }

        public EmailTemplate(string content)
        {
            Content = content;
        }
    }
}