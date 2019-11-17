using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.Emailing.Templates
{
    public class EmailTemplateContributorList : List<IEmailTemplateContributor>
    {
        public string GetOrNull(string cultureName)
        {
            foreach (var contributor in this.AsQueryable().Reverse())
            {
                var templateString = contributor.GetOrNull(cultureName);
                if (templateString != null)
                {
                    return templateString;
                }
            }

            return null;
        }
    }
}