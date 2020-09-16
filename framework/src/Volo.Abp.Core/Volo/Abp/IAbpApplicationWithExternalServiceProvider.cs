using System;
using JetBrains.Annotations;

namespace Volo.Abp
{
    public interface IAbpApplicationWithExternalServiceProvider : IAbpApplication
    {
        void Initialize([NotNull] IServiceProvider serviceProvider);
    }
}
