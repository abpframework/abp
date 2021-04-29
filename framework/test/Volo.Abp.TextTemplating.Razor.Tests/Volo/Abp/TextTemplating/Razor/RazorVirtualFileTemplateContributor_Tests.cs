using System;
using Volo.Abp.TextTemplating.VirtualFiles;

namespace Volo.Abp.TextTemplating.Razor
{
    public class RazorVirtualFileTemplateContributor_Tests : VirtualFileTemplateContributor_Tests<RazorTextTemplatingTestModule>
    {
        public RazorVirtualFileTemplateContributor_Tests()
        {
            WelcomeEmailEnglishContent = "@inherits Volo.Abp.TextTemplating.Razor.RazorTemplatePageBase<Volo.Abp.TextTemplating.Razor.RazorTemplateRenderer_Tests.WelcomeEmailModel>" +
                                         Environment.NewLine +
                                         "Welcome @Model.Name to the abp.io!";

            WelcomeEmailTurkishContent = "@inherits Volo.Abp.TextTemplating.Razor.RazorTemplatePageBase<Volo.Abp.TextTemplating.Razor.RazorTemplateRenderer_Tests.WelcomeEmailModel>" +
                                         Environment.NewLine +
                                         "Merhaba @Model.Name, abp.io'ya hoşgeldiniz!";

            ForgotPasswordEmailEnglishContent = "@inherits Volo.Abp.TextTemplating.Razor.RazorTemplatePageBase<Volo.Abp.TextTemplating.Razor.RazorTemplateRenderer_Tests.ForgotPasswordEmailModel>" +
                                                Environment.NewLine +
                                                "@Localizer[\"HelloText\", Model.Name], @Localizer[\"HowAreYou\"]. Please click to the following link to get an email to reset your password!";
        }
    }
}
