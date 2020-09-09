using System.Threading.Tasks;

namespace Volo.Docs.Documents
{
    public interface INavigationTreePostProcessor
    {
        Task ProcessAsync(NavigationTreePostProcessorContext context);
    }
}