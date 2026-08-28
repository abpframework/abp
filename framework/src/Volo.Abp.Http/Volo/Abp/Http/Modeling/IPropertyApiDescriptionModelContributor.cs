using System.Reflection;

namespace Volo.Abp.Http.Modeling;

public interface IPropertyApiDescriptionModelContributor
{
    void Contribute(PropertyApiDescriptionModel model, PropertyInfo propertyInfo);
}
