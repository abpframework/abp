using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Microsoft.Extensions.Localization;

public interface IAbpStringLocalizerFactory
{
    [CanBeNull]
    IStringLocalizer CreateDefaultOrNull();

    [CanBeNull]
    IStringLocalizer CreateByResourceNameOrNull([NotNull] string resourceName);
    
    [ItemCanBeNull]
    Task<IStringLocalizer> CreateByResourceNameOrNullAsync([NotNull] string resourceName);
}
