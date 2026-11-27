using System.Threading.Tasks;
using OpenIddict.Abstractions;
using Shouldly;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.OpenIddict.Tokens;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.OpenIddict.MongoDB;

[Collection(MongoTestCollection.Name)]
public class OpenIddictRevokeTransaction_Tests : OpenIddictMongoDbTestBase
{
    private readonly IOpenIddictTokenRepository _tokenRepository;
    private readonly IOpenIddictAuthorizationRepository _authorizationRepository;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly AbpOpenIddictTestData _testData;

    public OpenIddictRevokeTransaction_Tests()
    {
        _tokenRepository = GetRequiredService<IOpenIddictTokenRepository>();
        _authorizationRepository = GetRequiredService<IOpenIddictAuthorizationRepository>();
        _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
        _testData = GetRequiredService<AbpOpenIddictTestData>();
    }

    [Fact]
    public async Task Token_Revoke_Should_Be_Rolled_Back_With_The_Unit_Of_Work()
    {
        using (_unitOfWorkManager.Begin(new AbpUnitOfWorkOptions { IsTransactional = true }))
        {
            (await _tokenRepository.RevokeBySubjectAsync(_testData.Subject2)).ShouldBe(1);
        }

        (await _tokenRepository.FindByIdAsync(_testData.Token2Id)).Status.ShouldBe(OpenIddictConstants.Statuses.Valid);
    }

    [Fact]
    public async Task Authorization_Revoke_Should_Be_Rolled_Back_With_The_Unit_Of_Work()
    {
        using (_unitOfWorkManager.Begin(new AbpUnitOfWorkOptions { IsTransactional = true }))
        {
            (await _authorizationRepository.RevokeBySubjectAsync(_testData.Subject1)).ShouldBe(1);
        }

        (await _authorizationRepository.FindByIdAsync(_testData.Authorization1Id)).Status.ShouldBe(OpenIddictConstants.Statuses.Valid);
    }
}
