using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs.DemoApp.Shared.Jobs
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
            _backgroundJobManager.Enqueue(new WriteToConsoleGreenJobArgs { Value = "test 1 (green)" });
            _backgroundJobManager.Enqueue(new WriteToConsoleGreenJobArgs { Value = "test 2 (green)" });
            _backgroundJobManager.Enqueue(new WriteToConsoleYellowJobArgs { Value = "test 1 (yellow)" });
            _backgroundJobManager.Enqueue(new WriteToConsoleYellowJobArgs { Value = "test 2 (yellow)" });
        }
    }
}
