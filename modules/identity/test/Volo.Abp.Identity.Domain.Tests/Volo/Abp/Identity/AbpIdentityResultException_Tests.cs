using Microsoft.AspNetCore.Identity;
using Shouldly;
using Volo.Abp.Localization;
using Xunit;

namespace Volo.Abp.Identity
{
    public class AbpIdentityResultException_Tests : AbpIdentityDomainTestBase
    {
        [Fact]
        public void Should_Localize_Messages()
        {
            var exception = new AbpIdentityResultException(
                IdentityResult.Failed(
                    new IdentityError
                    {
                        Code = "PasswordTooShort",
                        Description = "Passwords must be at least 6 characters."
                    },
                    new IdentityError
                    {
                        Code = "PasswordRequiresNonAlphanumeric",
                        Description = "Passwords must have at least one non alphanumeric character."
                    }
                )
            );

            using (CultureHelper.Use("tr"))
            {
                var localizeMessage = exception.LocalizeMessage(new LocalizationContext(ServiceProvider));

                localizeMessage.ShouldContain("Şifre uzunluğu 6 karakterden uzun olmalıdır.");
                localizeMessage.ShouldContain("Parola en az bir alfasayısal olmayan karakter içermeli");
            }

            using (CultureHelper.Use("en"))
            {
                var localizeMessage = exception.LocalizeMessage(new LocalizationContext(ServiceProvider));

                localizeMessage.ShouldContain("Password length must be greater than 6 characters.");
                localizeMessage.ShouldContain("Password must contain at least one non-alphanumeric character.");
            }
        }
    }
}
