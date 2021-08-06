using System.Threading.Tasks;

namespace Volo.Abp.Validation
{
    public interface IObjectValidationContributor
    {
        Task AddErrorsAsync(ObjectValidationContext context);
    }
}
