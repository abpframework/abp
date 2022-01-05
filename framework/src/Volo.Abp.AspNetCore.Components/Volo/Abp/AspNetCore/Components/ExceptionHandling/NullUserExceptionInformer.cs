using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.ExceptionHandling;

public class NullUserExceptionInformer : IUserExceptionInformer, ISingletonDependency
{
    public void Inform(UserExceptionInformerContext context)
    {

    }

    public Task InformAsync(UserExceptionInformerContext context)
    {
        return Task.CompletedTask;
    }
}
