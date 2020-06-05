using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.ExceptionHandling
{
    [ExposeServices(typeof(IExceptionSubscriber))]
    public abstract class ExceptionSubscriber : IExceptionSubscriber, ITransientDependency
    {
        public abstract Task HandleAsync(ExceptionNotificationContext context);
    }
}