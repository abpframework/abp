using Microsoft.AspNetCore.Identity;
using Shouldly;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Localization;
using Xunit;

namespace Volo.Abp.Identity;

public class AbpIdentityResultException_Tests : AbpIdentityDomainTestBase
{
    [Fact]
    public void Should_Localize_Messages()
    {
        var describer = GetRequiredService<IdentityErrorDescriber>();
        var exceptionToErrorInfoConverter = GetRequiredService<IExceptionToErrorInfoConverter>();

        using (CultureHelper.Use("en"))
        {
            var exception = new AbpIdentityResultException(
                IdentityResult.Failed(
                    describer.PasswordTooShort(6),
                    describer.PasswordRequiresNonAlphanumeric(),
                    describer.DefaultError())
            );

            var message = exception.LocalizeMessage(new LocalizationContext(ServiceProvider));
            exceptionToErrorInfoConverter.Convert(exception).Message.ShouldBe(message);

            message.ShouldNotBeNull();
            message.ShouldContain("Password length must be greater than 6 characters.");
            message.ShouldContain("Password must contain at least one non-alphanumeric character.");
            message.ShouldContain("An unknown failure has occurred.");
        }

        using (CultureHelper.Use("tr"))
        {
            var exception = new AbpIdentityResultException(
                IdentityResult.Failed(
                    describer.PasswordTooShort(6),
                    describer.PasswordRequiresNonAlphanumeric(),
                    describer.DefaultError())
            );

            var message = exception.LocalizeMessage(new LocalizationContext(ServiceProvider));
            exceptionToErrorInfoConverter.Convert(exception).Message.ShouldBe(message);

            message.ShouldNotBeNull();
            message.ShouldContain("Şifre uzunluğu 6 karakterden uzun olmalıdır.");
            message.ShouldContain("Parola en az bir alfasayısal olmayan karakter içermeli");
            message.ShouldContain("Bilinmeyen bir hata oluştu.");
        }

        using (CultureHelper.Use("zh-Hans"))
        {
            var exception = new AbpIdentityResultException(
                IdentityResult.Failed(
                    describer.PasswordTooShort(6),
                    describer.PasswordRequiresNonAlphanumeric(),
                    describer.DefaultError())
            );

            var message = exception.LocalizeMessage(new LocalizationContext(ServiceProvider));
            exceptionToErrorInfoConverter.Convert(exception).Message.ShouldBe(message);

            message.ShouldNotBeNull();
            message.ShouldContain("密码长度必须大于 6 字符。");
            message.ShouldContain("密码必须至少包含一个非字母数字字符。");
            message.ShouldContain("发生了一个未知错误。");
        }
    }
}
