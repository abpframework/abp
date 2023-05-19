using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.ExceptionHandling;

public interface IExceptionSubscriber
{
    Task HandleAsync([NotNull] ExceptionNotificationContext context);
}
