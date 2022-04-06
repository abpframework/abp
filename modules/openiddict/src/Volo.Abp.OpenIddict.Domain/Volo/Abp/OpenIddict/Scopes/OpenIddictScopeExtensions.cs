namespace Volo.Abp.OpenIddict.Scopes;

public static class OpenIddictScopeExtensions
{
    public static OpenIddictScope ToEntity(this OpenIddictScopeModel model)
    {
        Check.NotNull(model, nameof(model));

        return new OpenIddictScope(model.Id)
        {
            Description = model.Description,
            Descriptions = model.Descriptions,
            DisplayName = model.DisplayName,
            DisplayNames = model.DisplayNames,
            Name = model.Name,
            Properties = model.Properties,
            Resources = model.Resources
        };
    }

    public static OpenIddictScope ToEntity(this OpenIddictScopeModel model, OpenIddictScope entity)
    {
        Check.NotNull(model, nameof(model));
        Check.NotNull(entity, nameof(entity));

        entity.Description = model.Description;
        entity.Descriptions = model.Descriptions;
        entity.DisplayName = model.DisplayName;
        entity.DisplayNames = model.DisplayNames;
        entity.Name = model.Name;
        entity.Properties = model.Properties;
        entity.Resources = model.Resources;

        return entity;
    }

    public static OpenIddictScopeModel ToModel(this OpenIddictScope model)
    {
        if(model == null)
        {
            return null;
        }

        return new OpenIddictScopeModel
        {
            Id = model.Id,
            Description = model.Description,
            Descriptions = model.Descriptions,
            DisplayName = model.DisplayName,
            DisplayNames = model.DisplayNames,
            Name = model.Name,
            Properties = model.Properties,
            Resources = model.Resources
        };
    }
}
