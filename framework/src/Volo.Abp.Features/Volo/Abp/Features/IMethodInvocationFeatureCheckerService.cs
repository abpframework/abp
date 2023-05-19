using System.Threading.Tasks;

namespace Volo.Abp.Features;

public interface IMethodInvocationFeatureCheckerService
{
    Task CheckAsync(
        MethodInvocationFeatureCheckerContext context
    );
}
