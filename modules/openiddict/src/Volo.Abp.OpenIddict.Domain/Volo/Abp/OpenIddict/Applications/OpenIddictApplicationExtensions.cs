namespace Volo.Abp.OpenIddict.Applications;

public static class OpenIddictApplicationExtensions
{
    public static OpenIddictApplication ToEntity(this OpenIddictApplicationModel model)
    {
        Check.NotNull(model, nameof(model));

        return new OpenIddictApplication(model.Id)
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

        return entity;
    }

    public static OpenIddictApplicationModel ToModel(this OpenIddictApplication model)
    {
        if(model == null)
        {
            return null;
        }

        return new OpenIddictApplicationModel
        {
            Id = model.Id,
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
    }
}
