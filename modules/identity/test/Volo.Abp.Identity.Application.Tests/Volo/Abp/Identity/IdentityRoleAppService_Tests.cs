using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Identity;

public class IdentityRoleAppService_Tests : AbpIdentityApplicationTestBase
{
    private readonly IIdentityRoleAppService _roleAppService;
    private readonly IIdentityRoleRepository _roleRepository;

    public IdentityRoleAppService_Tests()
    {
        _roleAppService = GetRequiredService<IIdentityRoleAppService>();
        _roleRepository = GetRequiredService<IIdentityRoleRepository>();
    }

    [Fact]
    public async Task GetAsync()
    {
        //Arrange

        var moderator = await GetRoleAsync("moderator");

        //Act

        var result = await _roleAppService.GetAsync(moderator.Id);

        //Assert

        result.Id.ShouldBe(moderator.Id);
    }

    [Fact]
    public async Task GetAllListAsync()
    {
        //Act

        var result = await _roleAppService.GetAllListAsync();

        //Assert

        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task GetListAsync()
    {
        //Act

        var result = await _roleAppService.GetListAsync(new GetIdentityRolesInput());

        //Assert

        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task CreateAsync()
    {
        //Arrange

        var input = new IdentityRoleCreateDto
        {
            Name = Guid.NewGuid().ToString("N").Left(8)
        };

        //Act

        var result = await _roleAppService.CreateAsync(input);

        //Assert

        result.Id.ShouldNotBe(Guid.Empty);
        result.Name.ShouldBe(input.Name);

        var role = await _roleRepository.GetAsync(result.Id);
        role.Name.ShouldBe(input.Name);
    }

    [Fact]
    public async Task UpdateAsync()
    {
        //Arrange

        var moderator = await GetRoleAsync("moderator");

        var input = new IdentityRoleUpdateDto
        {
            Name = Guid.NewGuid().ToString("N").Left(8),
            ConcurrencyStamp = moderator.ConcurrencyStamp,
            IsDefault = moderator.IsDefault,
            IsPublic = moderator.IsPublic
        };

        //Act

        var result = await _roleAppService.UpdateAsync(moderator.Id, input);

        //Assert

        result.Id.ShouldBe(moderator.Id);
        result.Name.ShouldBe(input.Name);

        var updatedRole = await _roleRepository.GetAsync(moderator.Id);
        updatedRole.Name.ShouldBe(input.Name);
    }

    [Fact]
    public async Task DeleteAsync()
    {
        //Arrange

        var moderator = await GetRoleAsync("moderator");

        //Act

        await _roleAppService.DeleteAsync(moderator.Id);

        //Assert

        (await FindRoleAsync("moderator")).ShouldBeNull();
    }

    private async Task<IdentityRole> GetRoleAsync(string roleName)
    {
        return (await _roleRepository.GetListAsync()).First(u => u.Name == roleName);
    }

    private async Task<IdentityRole> FindRoleAsync(string roleName)
    {
        return (await _roleRepository.GetListAsync()).FirstOrDefault(u => u.Name == roleName);
    }
}
