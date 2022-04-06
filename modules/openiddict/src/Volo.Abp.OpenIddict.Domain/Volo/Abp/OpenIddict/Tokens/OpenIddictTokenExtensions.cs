namespace Volo.Abp.OpenIddict.Tokens;

public static class OpenIddictTokenExtensions
{
    public static OpenIddictToken ToEntity(this OpenIddictTokenModel model)
    {
        Check.NotNull(model, nameof(model));

        return new OpenIddictToken(model.Id)
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

        return entity;
    }

    public static OpenIddictTokenModel ToModel(this OpenIddictToken model)
    {
        if(model == null)
        {
            return null;
        }

        return new OpenIddictTokenModel
        {
            Id = model.Id,
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
    }
}
