using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Abp.Http.Modeling;

namespace Volo.Abp.Http.DynamicProxying;

public class TestObjectToPath : IObjectToPath<int>, ITransientDependency
{
    public Task<string> ConvertAsync(ActionApiDescriptionModel actionApiDescription, ParameterApiDescriptionModel parameterApiDescription, int value)
    {
        if (actionApiDescription.Name == nameof(IRegularTestController.GetObjectandCountAsync))
        {
            if (value <= 0)
            {
                value = 888;
            }
            return Task.FromResult(value.ToString());
        }

        return Task.FromResult<string>(null);
    }
}
