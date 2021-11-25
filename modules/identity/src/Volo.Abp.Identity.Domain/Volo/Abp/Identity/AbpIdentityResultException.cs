using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;

namespace Volo.Abp.Identity;

[Serializable]
public class AbpIdentityResultException : BusinessException, ILocalizeErrorMessage
{
    public IdentityResult IdentityResult { get; }

    public AbpIdentityResultException([NotNull] IdentityResult identityResult)
        : base(
            code: $"Volo.Abp.Identity:{identityResult.Errors.First().Code}",
            message: identityResult.Errors.Select(err => err.Description).JoinAsString(", "))
    {
        IdentityResult = Check.NotNull(identityResult, nameof(identityResult));
    }

    public AbpIdentityResultException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {

    }

    public virtual string LocalizeMessage(LocalizationContext context)
    {
        var localizer = context.LocalizerFactory.Create<IdentityResource>();

        SetData(localizer);

        return IdentityResult.LocalizeErrors(localizer);
    }

    protected virtual void SetData(IStringLocalizer localizer)
    {
        var values = IdentityResult.GetValuesFromErrorMessage(localizer);

        for (var index = 0; index < values.Length; index++)
        {
            Data[index.ToString()] = values[index];
        }
    }
}
