using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs.DemoApp.Jobs
{
    public class SampleJobCreator : ITransientDependency
    {
        private readonly IBackgroundJobManager _backgroundJobManager;

        public SampleJobCreator(IBackgroundJobManager backgroundJobManager)
        {
            _backgroundJobManager = backgroundJobManager;
        }

        public void CreateJobs()
        {
            _backgroundJobManager.Enqueue(new WriteToConsoleJobArgs { Value = "42" });
            _backgroundJobManager.Enqueue(new WriteToConsoleJobArgs { Value = "43" });
        }
    }
}
