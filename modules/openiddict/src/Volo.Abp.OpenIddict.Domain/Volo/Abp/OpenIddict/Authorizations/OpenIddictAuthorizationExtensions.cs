namespace Volo.Abp.OpenIddict.Authorizations;

public static class OpenIddictAuthorizationExtensions
{
    public static OpenIddictAuthorization ToEntity(this OpenIddictAuthorizationModel model)
    {
        Check.NotNull(model, nameof(model));

        var entity = new OpenIddictAuthorization(model.Id)
        {
            ApplicationId = model.ApplicationId,
            CreationDate = model.CreationDate,
            Properties = model.Properties,
            Scopes = model.Scopes,
            Status = model.Status,
            Subject = model.Subject,
            Type = model.Type
        };

        foreach (var extraProperty in model.ExtraProperties)
        {
            entity.ExtraProperties.Remove(extraProperty.Key);
            entity.ExtraProperties.Add(extraProperty.Key, extraProperty.Value);
        }

        return entity;
    }

    public static OpenIddictAuthorization ToEntity(this OpenIddictAuthorizationModel model, OpenIddictAuthorization entity)
    {
        Check.NotNull(model, nameof(model));
        Check.NotNull(entity, nameof(entity));

        entity.ApplicationId = model.ApplicationId;
        entity.CreationDate = model.CreationDate;
        entity.Properties = model.Properties;
        entity.Scopes = model.Scopes;
        entity.Status = model.Status;
        entity.Subject = model.Subject;
        entity.Type = model.Type;

        foreach (var extraProperty in model.ExtraProperties)
        {
            entity.ExtraProperties.Remove(extraProperty.Key);
            entity.ExtraProperties.Add(extraProperty.Key, extraProperty.Value);
        }

        return entity;
    }

    public static OpenIddictAuthorizationModel ToModel(this OpenIddictAuthorization entity)
    {
        if(entity == null)
        {
            return null;
        }

        var model = new OpenIddictAuthorizationModel
        {
            Id = entity.Id,
            ApplicationId = entity.ApplicationId,
            CreationDate = entity.CreationDate,
            Properties = entity.Properties,
            Scopes = entity.Scopes,
            Status = entity.Status,
            Subject = entity.Subject,
            Type = entity.Type
        };

        foreach (var extraProperty in entity.ExtraProperties)
        {
            model.ExtraProperties.Remove(extraProperty.Key);
            model.ExtraProperties.Add(extraProperty.Key, extraProperty.Value);
        }

        return model;
    }
}
