using System;
using System.Collections.Generic;
using Volo.Abp.Content;
using Volo.Abp.Reflection;
using Volo.Abp.Threading;

namespace Volo.Abp.Http.Modeling;

[Serializable]
public class ReturnValueApiDescriptionModel
{
    public string Type { get; set; } = default!;

    public string TypeSimple { get; set; } = default!;

    public string? Summary { get; set; }

    public IList<string>? ContentTypes { get; set; }

    public bool IsRemoteStream { get; set; }

    public ReturnValueApiDescriptionModel()
    {

    }

    public static ReturnValueApiDescriptionModel Create(Type type, IList<string>? contentTypes = null)
    {
        var unwrappedType = AsyncHelper.UnwrapTask(type);

        return new ReturnValueApiDescriptionModel
        {
            Type = TypeHelper.GetFullNameHandlingNullableAndGenerics(unwrappedType),
            TypeSimple = ApiTypeNameHelper.GetSimpleTypeName(unwrappedType),
            ContentTypes = contentTypes,
            IsRemoteStream = IsRemoteStreamType(unwrappedType)
        };
    }

    private static bool IsRemoteStreamType(Type type)
    {
        return type == typeof(IRemoteStreamContent) || type == typeof(RemoteStreamContent);
    }
}
