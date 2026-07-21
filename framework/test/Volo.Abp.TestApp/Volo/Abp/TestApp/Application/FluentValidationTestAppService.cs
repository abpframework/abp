using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.TestApp.Application;

public class FluentValidationTestAppService : ApplicationService
{
    public virtual Task<string> CreateAsync(FluentValidationTestInput input)
    {
        return Task.FromResult(input.Name);
    }
}

public class FluentValidationTestInput
{
    public string Name { get; set; }
}
