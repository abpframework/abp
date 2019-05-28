using System.Threading.Tasks;

namespace Volo.Abp.Features
{
    public interface IMethodFeatureTestService
    {
        Task<int> Feature1Async();

        Task NonFeatureAsync();
    }
}