using System.Threading.Tasks;

namespace Volo.Abp.Http.DynamicProxying
{
    public interface IRegularTestController
    {
        int IncrementValue(int value);

        Task<int> IncrementValueAsync(int value);
    }
}
