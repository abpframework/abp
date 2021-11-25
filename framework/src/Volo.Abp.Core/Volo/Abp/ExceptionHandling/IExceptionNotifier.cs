using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.ExceptionHandling;

public interface IExceptionNotifier
{
    Task NotifyAsync([NotNull] ExceptionNotificationContext context);
}
