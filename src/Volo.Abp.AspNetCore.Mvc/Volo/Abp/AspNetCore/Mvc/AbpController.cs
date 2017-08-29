using Microsoft.AspNetCore.Mvc;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc
{
    public abstract class AbpController : Controller, ITransientDependency
    {

    }
}
