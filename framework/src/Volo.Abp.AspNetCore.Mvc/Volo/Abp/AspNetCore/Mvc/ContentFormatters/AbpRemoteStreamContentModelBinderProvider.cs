using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Volo.Abp.Content;

namespace Volo.Abp.AspNetCore.Mvc.ContentFormatters;

public class AbpRemoteStreamContentModelBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (context.Metadata.ModelType == typeof(IRemoteStreamContent) ||
            context.Metadata.ModelType == typeof(RemoteStreamContent) ||
            typeof(IEnumerable<IRemoteStreamContent>).IsAssignableFrom(context.Metadata.ModelType) ||
            typeof(IEnumerable<RemoteStreamContent>).IsAssignableFrom(context.Metadata.ModelType))
        {
            return new AbpRemoteStreamContentModelBinder();
        }

        return null;
    }
}
