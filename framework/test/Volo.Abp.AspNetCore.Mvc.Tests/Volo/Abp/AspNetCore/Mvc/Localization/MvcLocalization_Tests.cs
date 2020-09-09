using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Shouldly;
using Volo.Abp.Localization;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Localization
{
    public class MvcLocalization_Tests : AspNetCoreMvcTestBase
    {
        private readonly IStringLocalizer<MvcLocalization_Tests> _localizer;

        public MvcLocalization_Tests()
        {
            _localizer = ServiceProvider.GetRequiredService<IStringLocalizer<MvcLocalization_Tests>>();
        }

        [Fact]
        public void Should_Get_Same_Text_If_Not_Defined_From_StringLocalizer()
        {
            const string text = "A string that is not defined!";

            _localizer[text].Value.ShouldBe(text);
        }

        [Fact]
        public async Task Should_Get_Same_Text_If_Not_Defined_In_Razor_View()
        {
            var result = await GetResponseAsStringAsync("/LocalizationTest/HelloJohn");
            result.ShouldBe("Hello <b>John</b>.");
        }
        
        [Fact]
        public async Task Should_Localize_Display_Attribute()
        {
            using (CultureHelper.Use("en"))
            {
                var result = await GetResponseAsStringAsync("/LocalizationTest/PersonForm");
                result.ShouldContain("<label for=\"BirthDate\">Birth date</label>");
            }

            using (CultureHelper.Use("tr"))
            {
                var result = await GetResponseAsStringAsync("/LocalizationTest/PersonForm");
                result.ShouldContain("<label for=\"BirthDate\">Dogum gunu</label>");
            }
        }
    }
}
