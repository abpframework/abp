using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.Components.Web;

public class MenuItemUrlHelper_Tests
{
    [Theory]
    [InlineData("/", "")]
    [InlineData("/books", "books")]
    [InlineData("~/books", "books")]
    [InlineData("books", "books")]
    [InlineData("/identity/users?filter=a", "identity/users?filter=a")]
    [InlineData("//cdn.example.com/help", "//cdn.example.com/help")]
    [InlineData("https://docs.example.com/help", "https://docs.example.com/help")]
    [InlineData("mailto:info@example.com", "mailto:info@example.com")]
    public void Should_Make_Local_Urls_Relative_To_Base(string url, string expected)
    {
        MenuItemUrlHelper.ToBaseRelativeUrl(url).ShouldBe(expected);
    }

    [Fact]
    public void Should_Return_Null_For_Null_Url()
    {
        MenuItemUrlHelper.ToBaseRelativeUrl(null).ShouldBeNull();
    }
}
