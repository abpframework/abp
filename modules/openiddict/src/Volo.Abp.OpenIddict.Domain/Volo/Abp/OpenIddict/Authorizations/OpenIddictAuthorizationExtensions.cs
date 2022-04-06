namespace Volo.Abp.OpenIddict.Authorizations;

public static class OpenIddictAuthorizationExtensions
{
    public static OpenIddictAuthorization ToEntity(this OpenIddictAuthorizationModel model)
    {
        Check.NotNull(model, nameof(model));

        return new OpenIddictAuthorization(model.Id)
        {
            ApplicationId = model.ApplicationId,
            CreationDate = model.CreationDate,
            Properties = model.Properties,
            Scopes = model.Scopes,
            Status = model.Status,
            Subject = model.Subject,
            Type = model.Type
        };
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

        return entity;
    }

    public static OpenIddictAuthorizationModel ToModel(this OpenIddictAuthorization model)
    {
        if(model == null)
        {
            return null;
        }

        return new OpenIddictAuthorizationModel
        {
            Id = model.Id,
            ApplicationId = model.ApplicationId,
            CreationDate = model.CreationDate,
            Properties = model.Properties,
            Scopes = model.Scopes,
            Status = model.Status,
            Subject = model.Subject,
            Type = model.Type
        };
    }
}
