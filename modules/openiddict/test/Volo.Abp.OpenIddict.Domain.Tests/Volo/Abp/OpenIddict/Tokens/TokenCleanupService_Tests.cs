using System.Threading.Tasks;
using OpenIddict.Abstractions;
using Shouldly;
using Xunit;

namespace Volo.Abp.OpenIddict.Tokens;

public class TokenCleanupService_Tests : OpenIddictDomainTestBase
{
    private readonly IOpenIddictTokenManager _tokenManager;
    private readonly IOpenIddictAuthorizationManager _authorizationManager;
    private readonly TokenCleanupService _tokenCleanupService;
    
    public TokenCleanupService_Tests()
    {
        _tokenManager = GetRequiredService<IOpenIddictTokenManager>();
        _authorizationManager = GetRequiredService<IOpenIddictAuthorizationManager>();
        _tokenCleanupService = GetRequiredService<TokenCleanupService>();
    }

    [Fact]
    public async Task Should_Clear_Expired_Tokens()
    {
        var tokensCount = await _tokenManager.CountAsync();
        var authorizationsCount = await _authorizationManager.CountAsync();

        await _tokenCleanupService.CleanAsync();

        (await _tokenManager.CountAsync())
            .ShouldBe(tokensCount - 1);

        (await _authorizationManager.CountAsync())
            .ShouldBe(authorizationsCount - 1);
    }
}