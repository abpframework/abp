using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Volo.Abp.Users;
using Volo.Abp.Validation;
using Xunit;

namespace Volo.Abp.Account;

public class ProfileAppService_Tests : AbpAccountApplicationTestBase
{
    private readonly IProfileAppService _profileAppService;
    private readonly AccountTestData _testData;
    private ICurrentUser _currentUser;

    public ProfileAppService_Tests()
    {
        _profileAppService = GetRequiredService<IProfileAppService>();
        _testData = GetRequiredService<AccountTestData>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        _currentUser = Substitute.For<ICurrentUser>();
        services.AddSingleton(_currentUser);
    }

    [Fact]
    public async Task GetAsync()
    {
        //Arrange
        _currentUser.Id.Returns(_testData.UserJohnId);
        _currentUser.IsAuthenticated.Returns(true);

        //Act
        var result = await _profileAppService.GetAsync();

        //Assert
        var johnNash = GetUser("john.nash");

        result.UserName.ShouldBe(johnNash.UserName);
        result.Email.ShouldBe(johnNash.Email);
        result.PhoneNumber.ShouldBe(johnNash.PhoneNumber);
    }


    [Fact]
    public async Task UpdateAsync()
    {
        //Arrange
        _currentUser.Id.Returns(_testData.UserJohnId);
        _currentUser.IsAuthenticated.Returns(true);

        var input = new UpdateProfileDto
        {
            UserName = CreateRandomString(),
            PhoneNumber = CreateRandomPhoneNumber(),
            Email = CreateRandomEmail(),
            Name = CreateRandomString(),
            Surname = CreateRandomString()
        };

        //Act
        var result = await _profileAppService.UpdateAsync(input);

        //Assert
        result.UserName.ShouldBe(input.UserName);
        result.Email.ShouldBe(input.Email);
        result.PhoneNumber.ShouldBe(input.PhoneNumber);
        result.Surname.ShouldBe(input.Surname);
        result.Name.ShouldBe(input.Name);
    }

    [Fact]
    public async Task Should_Not_UpdatePhoneNumber_If_Input_PhoneNumber_IsNullOrWhiteSpace_Test()
    {
        //Arrange
        _currentUser.Id.Returns(_testData.UserJohnId);
        _currentUser.IsAuthenticated.Returns(true);

        //Act
        var result = await _profileAppService.UpdateAsync(new UpdateProfileDto
        {
            UserName = CreateRandomString(),
            Email = CreateRandomEmail(),
            PhoneNumber = ""
        });

        //Assert
        result.PhoneNumber.ShouldBe(null);

        //Act
        result = await _profileAppService.UpdateAsync(new UpdateProfileDto
        {
            UserName = CreateRandomString(),
            Email = CreateRandomEmail(),
            PhoneNumber = "123"
        });

        //Assert
        result.PhoneNumber.ShouldBe("123");
    }

    [Fact]
    public async Task ChangePasswordAsync_FailsForSamePassword()
    {
        //Arrange
        _currentUser.Id.Returns(_testData.UserJohnId);
        _currentUser.IsAuthenticated.Returns(true);

        //Act
        var ex = await _profileAppService.ChangePasswordAsync(new()
        {
            CurrentPassword = "SomePassword123!",
            NewPassword = "SomePassword123!"
        }).ShouldThrowAsync<AbpValidationException>();

        //Assert
        ex.ValidationErrors.ShouldNotBeEmpty();
        var firstError = ex.ValidationErrors[0];
        firstError.MemberNames.ShouldContain(nameof(ChangePasswordInput.CurrentPassword));
        firstError.MemberNames.ShouldContain(nameof(ChangePasswordInput.NewPassword));
    }

    private static string CreateRandomEmail()
    {
        return CreateRandomString() + "@abp.io";
    }

    private static string CreateRandomString()
    {
        return Guid.NewGuid().ToString("N").Left(16);
    }

    private static string CreateRandomPhoneNumber()
    {
        return RandomHelper.GetRandom(10000000, 100000000).ToString();
    }
}
