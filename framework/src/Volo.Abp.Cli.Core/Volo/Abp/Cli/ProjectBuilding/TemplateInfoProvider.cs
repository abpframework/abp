using System;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Templates;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectBuilding
{
    public class TemplateInfoProvider : ITemplateInfoProvider, ITransientDependency
    {
        public TemplateInfo GetDefault()
        {
            return Get(MvcApplicationTemplate.TemplateName);
        }

        public TemplateInfo Get(string name)
        {
            switch (name)
            {
                case MvcApplicationTemplate.TemplateName:
                    return new MvcApplicationTemplate();
                case MvcModuleTemplate.TemplateName:
                    return new MvcModuleTemplate();
                case ServiceTemplate.TemplateName:
                    return new ServiceTemplate();
                default:
                    throw new Exception("There is no template found with given name: " + name);
            }
        }
    }
}