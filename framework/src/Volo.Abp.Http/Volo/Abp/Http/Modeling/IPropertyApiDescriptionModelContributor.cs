using System.Threading.Tasks;

namespace Volo.Abp.Http.Modeling;

public interface IPropertyApiDescriptionModelContributor
{
    Task ContributeAsync(PropertyApiDescriptionModelContributionContext context);
}
