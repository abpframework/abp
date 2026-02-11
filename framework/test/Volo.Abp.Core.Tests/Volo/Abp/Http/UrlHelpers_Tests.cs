using Shouldly;
using Xunit;

namespace Volo.Abp.Http;

public class UrlHelpers_Tests
{
    [Theory]
    [InlineData(null)]
    [InlineData("null")]
    [InlineData("http://")]
    [InlineData("http://*")]
    [InlineData("http://.domain")]
    [InlineData("http://.domain/hello")]
    public void IsSubdomainOf_ReturnsFalseIfDomainIsMalformedUri(string domain)
    {
        var actual = UrlHelpers.IsSubdomainOf("http://*.domain", domain);
        actual.ShouldBeFalse();
    }

    [Theory]
    [InlineData("http://sub.domain", "http://domain")]
    [InlineData("http://sub.domain", "http://*.domain")]
    [InlineData("http://sub.sub.domain", "http://*.domain")]
    [InlineData("http://sub.sub.domain", "http://*.sub.domain")]
    [InlineData("http://sub.domain:4567", "http://*.domain:4567")]
    public void IsSubdomainOf_ReturnsTrue_WhenASubdomain(string subdomain, string domain)
    {
        var actual = UrlHelpers.IsSubdomainOf(subdomain, domain);
        actual.ShouldBeTrue();
    }

    [Theory]
    [InlineData("http://sub.domain:1234", "http://*.domain:5678")]
    [InlineData("http://sub.domain", "http://domain.*")]
    [InlineData("http://sub.domain.hacker", "http://*.domain")]
    [InlineData("https://sub.domain", "http://*.domain")]
    public void IsSubdomainOf_ReturnsFalse_WhenNotASubdomain(string subdomain, string domain)
    {
        var actual = UrlHelpers.IsSubdomainOf(subdomain, domain);
        actual.ShouldBeFalse();
    }
}
