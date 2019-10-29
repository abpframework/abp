using System;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Templates.App;
using Volo.Abp.Cli.ProjectBuilding.Templates.MvcModule;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectBuilding
{
    public class TemplateInfoProvider : ITemplateInfoProvider, ITransientDependency
    {
        public TemplateInfo GetDefault()
        {
            return Get(AppTemplate.TemplateName);
        }

        public TemplateInfo Get(string name)
        {
            switch (name)
            {
                case AppTemplate.TemplateName:
                    return new AppTemplate();
                case AppProTemplate.TemplateName:
                    return new AppProTemplate();
                case ModuleTemplate.TemplateName:
                    return new ModuleTemplate();
                case ModuleProTemplate.TemplateName:
                    return new ModuleProTemplate();
                default:
                    throw new Exception("There is no template found with given name: " + name);
            }
        }
    }
}