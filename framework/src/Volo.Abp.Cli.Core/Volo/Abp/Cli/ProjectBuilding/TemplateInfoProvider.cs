using System;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.Cli.ProjectBuilding.Templates;
using Volo.Abp.Cli.ProjectBuilding.Templates.Mvc;
using Volo.Abp.Cli.ProjectBuilding.Templates.MvcModule;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.ProjectBuilding
{
    public class TemplateInfoProvider : ITemplateInfoProvider, ITransientDependency
    {
        public TemplateInfo GetDefault()
        {
            return Get(MvcTemplate.TemplateName);
        }

        public TemplateInfo Get(string name)
        {
            switch (name)
            {
                case MvcTemplate.TemplateName:
                    return new MvcTemplate();
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