using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Localization;

namespace Volo.Abp.Identity;

public class AbpIdentityResultException : BusinessException, ILocalizeErrorMessage
{
    public IdentityResult IdentityResult { get; }

    public AbpIdentityResultException([NotNull] IdentityResult identityResult)
        : base(message: identityResult.Errors.Select(err => err.Description).JoinAsString(", "))
    {
        IdentityResult = Check.NotNull(identityResult, nameof(identityResult));
    }

    public string LocalizeMessage(LocalizationContext context)
    {
        return Message;
    }
}
