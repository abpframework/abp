using JetBrains.Annotations;

namespace Volo.Abp.Tracing
{
    public interface ICorrelationIdProvider
    {
        [NotNull]
        string Get();
    }
}
