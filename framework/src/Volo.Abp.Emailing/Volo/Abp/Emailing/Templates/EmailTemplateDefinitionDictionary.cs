using System.Collections.Generic;

namespace Volo.Abp.Emailing.Templates
{
    public class EmailTemplateDefinitionDictionary : Dictionary<string, EmailTemplateDefinition>
    {
        public EmailTemplateDefinitionDictionary Add(EmailTemplateDefinition emailTemplateDefinition)
        {
            if (ContainsKey(emailTemplateDefinition.Name))
            {
                throw new AbpException(
                    "There is already an email template definition with given name: " +
                    emailTemplateDefinition.Name
                );
            }

            this[emailTemplateDefinition.Name] = emailTemplateDefinition;

            return this;
        }
    }
}