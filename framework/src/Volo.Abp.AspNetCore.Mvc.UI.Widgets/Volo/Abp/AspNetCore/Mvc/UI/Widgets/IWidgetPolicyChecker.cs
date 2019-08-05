using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Widgets
{
    public interface IWidgetPolicyChecker : ITransientDependency
    {
        Task<bool> CheckAsync(Type widgetComponentType);

        Task<bool> CheckAsync(string name);
    }
}
