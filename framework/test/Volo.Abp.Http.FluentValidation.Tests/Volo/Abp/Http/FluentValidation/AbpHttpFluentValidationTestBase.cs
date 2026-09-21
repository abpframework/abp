using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;

namespace Volo.Abp.Http.FluentValidation;

public abstract class AbpHttpFluentValidationTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected virtual async Task<PropertyApiDescriptionModel> GetPropertyAsync<TDto>(string propertyName)
    {
        var typeModel = await CreateTypeModelAsync(typeof(TDto));
        return typeModel.Properties!.Single(x => x.Name == propertyName);
    }

    protected virtual async Task<TypeApiDescriptionModel> CreateTypeModelAsync(Type type)
    {
        var typeModel = TypeApiDescriptionModel.Create(type);
        var contributors = ServiceProvider.GetServices<IPropertyApiDescriptionModelContributor>().ToArray();

        var propertyInfos = type
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.DeclaringType == type)
            .ToDictionary(p => p.Name, p => p);

        foreach (var propertyModel in typeModel.Properties!)
        {
            var context = new PropertyApiDescriptionModelContributionContext(propertyModel, propertyInfos[propertyModel.Name], type);
            foreach (var contributor in contributors)
            {
                await contributor.ContributeAsync(context);
            }
        }

        return typeModel;
    }
}
