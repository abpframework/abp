using System.Threading.Tasks;

namespace Volo.Abp.Threading
{
    public static class TaskCache
    {
        public static Task<bool> TrueResult { get; }
        public static Task<bool> FalseResult { get; }

        static TaskCache()
        {
            TrueResult = Task.FromResult(true);
            FalseResult = Task.FromResult(false);
        }
    }
}
