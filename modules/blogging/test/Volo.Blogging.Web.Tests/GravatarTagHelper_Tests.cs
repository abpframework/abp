using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Shouldly;
using Volo.Blogging.Areas.Blog.Helpers.TagHelpers;
using Xunit;

namespace Volo.Blogging;

public class GravatarTagHelper_Tests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Should_Render_A_Default_Avatar_When_The_Author_Email_Is_Missing(string email)
    {
        var helper = new GravatarTagHelper(new HttpContextAccessor()) { Email = email };
        var context = new TagHelperContext(new TagHelperAttributeList(), new Dictionary<object, object>(), "avatar");
        var output = new TagHelperOutput("img", new TagHelperAttributeList(),
            (useCached, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        helper.Process(context, output);

        output.Attributes["src"].Value.ToString().ShouldContain("https://secure.gravatar.com/avatar/d41d8cd98f00b204e9800998ecf8427e");
    }
}
