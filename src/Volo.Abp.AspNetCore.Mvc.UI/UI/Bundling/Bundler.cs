using System.Collections.Generic;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling
{
    public class Bundler : IBundler, ITransientDependency
    {
        public Bundler()
        {
            
        }

        public string CreateBundle(List<string> files)
        {
            return "";
        }
    }
}