using System.Text;

namespace Volo.Abp.Emailing.Templates
{
    public class EmailTemplate
    {
        public EmailTemplateDefinition Definition { get; }

        public string Content => ContentBuilder.ToString();

        protected StringBuilder ContentBuilder { get; set; }

        public EmailTemplate(string content, EmailTemplateDefinition definition)
        {
            ContentBuilder = new StringBuilder(content);
            Definition = definition;
        }

        public virtual void SetLayout(EmailTemplate layoutTemplate)
        {
            if (!layoutTemplate.Definition.IsLayout)
            {
                throw new AbpException($"Given template is not a layout template: {layoutTemplate.Definition.Name}");
            }

            var newStrBuilder = new StringBuilder(layoutTemplate.Content);
            newStrBuilder.Replace("{{#content}}", ContentBuilder.ToString());

            ContentBuilder = newStrBuilder;
        }

        public virtual void SetContent(string content)
        {
            ContentBuilder = new StringBuilder(content);
        }

        public virtual void Replace(string name, string value)
        {
            ContentBuilder.Replace("{{" + name + "}}", value);
        }
    }
}