using System.Collections.Generic;

namespace Volo.Abp.Emailing.Templates.Virtual
{
    public class VirtualFileEmailTemplateOptions
    {
        public IDictionary<string, string> Templates { get; }

        public VirtualFileEmailTemplateOptions()
        {
            Templates = new Dictionary<string, string>();
        }
    }
}