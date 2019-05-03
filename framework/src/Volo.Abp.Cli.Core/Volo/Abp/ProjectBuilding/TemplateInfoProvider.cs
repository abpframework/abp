using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ProjectBuilding.Templates;
using Volo.Abp.SolutionTemplating.Building;

namespace Volo.Abp.ProjectBuilding
{
    public class TemplateInfoProvider : ITemplateInfoProvider, ITransientDependency
    {
        public TemplateInfo GetDefault()
        {
            return Get(TemplateNames.Mvc);
        }

        public TemplateInfo Get(string name)
        {
            switch (name)
            {
                case TemplateNames.Mvc:
                    return new MvcApplicationTemplate();
                case TemplateNames.MvcModule:
                    return new MvcModuleTemplate();
                case TemplateNames.Service:
                    return new ServiceTemplate();
                default:
                    throw new Exception("There is no template found with given name: " + name);
            }
        }

        public static class TemplateNames
        {
            /// <summary>
            /// "mvc".
            /// </summary>
            public const string Mvc = "mvc";

            /// <summary>
            /// "mvcmodule".
            /// </summary>
            public const string MvcModule = "mvcmodule";

            /// <summary>
            /// "service".
            /// </summary>
            public const string Service = "service";
        }
    }
}