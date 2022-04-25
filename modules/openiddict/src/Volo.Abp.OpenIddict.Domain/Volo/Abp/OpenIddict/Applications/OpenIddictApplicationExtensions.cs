namespace Volo.Abp.OpenIddict.Applications;

public static class OpenIddictApplicationExtensions
{
    public static OpenIddictApplication ToEntity(this OpenIddictApplicationModel model)
    {
        Check.NotNull(model, nameof(model));

        var entity = new OpenIddictApplication(model.Id)
        {
            ClientId = model.ClientId,
            ClientSecret = model.ClientSecret,
            ConsentType = model.ConsentType,
            DisplayName = model.DisplayName,
            DisplayNames = model.DisplayNames,
            Permissions = model.Permissions,
            PostLogoutRedirectUris = model.PostLogoutRedirectUris,
            Properties = model.Properties,
            RedirectUris = model.RedirectUris,
            Requirements = model.Requirements,
            Type = model.Type
        };

        foreach (var extraProperty in model.ExtraProperties)
        {
            entity.ExtraProperties.Add(extraProperty.Key, extraProperty.Value);
        }

        return entity;
    }

    public static OpenIddictApplication ToEntity(this OpenIddictApplicationModel model, OpenIddictApplication entity)
    {
        Check.NotNull(model, nameof(model));
        Check.NotNull(entity, nameof(entity));

        entity.ClientId = model.ClientId;
        entity.ClientSecret = model.ClientSecret;
        entity.ConsentType = model.ConsentType;
        entity.DisplayName = model.DisplayName;
        entity.DisplayNames = model.DisplayNames;
        entity.Permissions = model.Permissions;
        entity.PostLogoutRedirectUris = model.PostLogoutRedirectUris;
        entity.Properties = model.Properties;
        entity.RedirectUris = model.RedirectUris;
        entity.Requirements = model.Requirements;
        entity.Type = model.Type;

        foreach (var extraProperty in model.ExtraProperties)
        {
            entity.ExtraProperties.Remove(extraProperty.Key);
            entity.ExtraProperties.Add(extraProperty.Key, extraProperty.Value);
        }

        return entity;
    }

    public static OpenIddictApplicationModel ToModel(this OpenIddictApplication entity)
    {
        if(entity == null)
        {
            return null;
        }

        var model = new OpenIddictApplicationModel
        {
            Id = entity.Id,
            ClientId = entity.ClientId,
            ClientSecret = entity.ClientSecret,
            ConsentType = entity.ConsentType,
            DisplayName = entity.DisplayName,
            DisplayNames = entity.DisplayNames,
            Permissions = entity.Permissions,
            PostLogoutRedirectUris = entity.PostLogoutRedirectUris,
            Properties = entity.Properties,
            RedirectUris = entity.RedirectUris,
            Requirements = entity.Requirements,
            Type = entity.Type
        };

        foreach (var extraProperty in entity.ExtraProperties)
        {
            model.ExtraProperties.Add(extraProperty.Key, extraProperty.Value);
        }

        return model;
    }
}
