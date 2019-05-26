using System.Threading.Tasks;

namespace MyCompanyName.MyProjectName.Samples
{
    public class SampleAppService : MyProjectNameAppServiceBase, ISampleAppService
    {
        public Task<SampleDto> GetAsync()
        {
            return Task.FromResult(
                new SampleDto
                {
                    Value = 42
                }
            );
        }
    }
}