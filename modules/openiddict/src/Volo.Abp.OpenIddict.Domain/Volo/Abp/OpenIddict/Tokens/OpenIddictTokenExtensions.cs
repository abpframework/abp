namespace Volo.Abp.OpenIddict.Tokens;

public static class OpenIddictTokenExtensions
{
    public static OpenIddictToken ToEntity(this OpenIddictTokenModel model)
    {
        Check.NotNull(model, nameof(model));

        var entity = new OpenIddictToken(model.Id)
        {
            ApplicationId = model.ApplicationId,
            AuthorizationId = model.AuthorizationId,
            CreationDate = model.CreationDate,
            ExpirationDate = model.ExpirationDate,
            Payload = model.Payload,
            Properties = model.Properties,
            RedemptionDate = model.RedemptionDate,
            ReferenceId = model.ReferenceId,
            Status = model.Status,
            Subject = model.Subject,
            Type = model.Type
        };


        foreach (var extraProperty in model.ExtraProperties)
        {
            entity.ExtraProperties.Add(extraProperty.Key, extraProperty.Value);
        }

        return entity;
    }

    public static OpenIddictToken ToEntity(this OpenIddictTokenModel model, OpenIddictToken entity)
    {
        Check.NotNull(model, nameof(model));
        Check.NotNull(entity, nameof(entity));

        entity.ApplicationId = model.ApplicationId;
        entity.AuthorizationId = model.AuthorizationId;
        entity.CreationDate = model.CreationDate;
        entity.ExpirationDate = model.ExpirationDate;
        entity.Payload = model.Payload;
        entity.Properties = model.Properties;
        entity.RedemptionDate = model.RedemptionDate;
        entity.ReferenceId = model.ReferenceId;
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

    public static OpenIddictTokenModel ToModel(this OpenIddictToken entity)
    {
        if(entity == null)
        {
            return null;
        }

        var model = new OpenIddictTokenModel
        {
            Id = entity.Id,
            ApplicationId = entity.ApplicationId,
            AuthorizationId = entity.AuthorizationId,
            CreationDate = entity.CreationDate,
            ExpirationDate = entity.ExpirationDate,
            Payload = entity.Payload,
            Properties = entity.Properties,
            RedemptionDate = entity.RedemptionDate,
            ReferenceId = entity.ReferenceId,
            Status = entity.Status,
            Subject = entity.Subject,
            Type = entity.Type
        };

        foreach (var extraProperty in entity.ExtraProperties)
        {
            model.ExtraProperties.Add(extraProperty.Key, extraProperty.Value);
        }

        return model;
    }
}
