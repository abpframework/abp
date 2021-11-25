using System;
using Volo.Abp.TextTemplating.VirtualFiles;

namespace Volo.Abp.TextTemplating.Razor;

public class RazorVirtualFileTemplateContributor_Tests : VirtualFileTemplateContributor_Tests<RazorTextTemplatingTestModule>
{
    public RazorVirtualFileTemplateContributor_Tests()
    {
        WelcomeEmailEnglishContent = "@inherits Volo.Abp.TextTemplating.Razor.RazorTemplatePageBase<Volo.Abp.TextTemplating.Razor.RazorTemplateRendererProvider_Tests.WelcomeEmailModel>" +
                                     Environment.NewLine +
                                     "Welcome @Model.Name to the abp.io!";

        WelcomeEmailTurkishContent = "@inherits Volo.Abp.TextTemplating.Razor.RazorTemplatePageBase<Volo.Abp.TextTemplating.Razor.RazorTemplateRendererProvider_Tests.WelcomeEmailModel>" +
                                     Environment.NewLine +
                                     "Merhaba @Model.Name, abp.io'ya hoşgeldiniz!";

        ForgotPasswordEmailEnglishContent = "@inherits Volo.Abp.TextTemplating.Razor.RazorTemplatePageBase<Volo.Abp.TextTemplating.Razor.RazorTemplateRendererProvider_Tests.ForgotPasswordEmailModel>" +
                                            Environment.NewLine +
                                            "@{" +
                                            Environment.NewLine +
                                            "    var url = @\"https://abp.io/Account/ResetPassword\";" +
                                            Environment.NewLine +
                                            "}" +
                                            Environment.NewLine +
                                            "@Localizer[\"HelloText\", Model.Name], @Localizer[\"HowAreYou\"]. Please click to the following link to get an email to reset your password!<a target=\"_blank\" href=\"@url\">Reset your password</a>";
    }
}
