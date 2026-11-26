using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Shouldly;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ClientProxies;
using Volo.Abp.Http.Client;
using Volo.Abp.Localization;
using Volo.Abp.Security.Claims;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Client;

public class MvcCachedApplicationConfigurationClient_Tests : AbpAspNetCoreMvcClientTestBase
{
    private AbpApplicationConfigurationClientProxy _configProxy;
    private AbpApplicationLocalizationClientProxy _localizationProxy;

    private readonly ICachedApplicationConfigurationClient _applicationConfigurationClient;

    public MvcCachedApplicationConfigurationClient_Tests()
    {
        _applicationConfigurationClient = GetRequiredService<ICachedApplicationConfigurationClient>();
    }

    protected override void AfterAddApplication(IServiceCollection services)
    {
        _configProxy = Substitute.For<AbpApplicationConfigurationClientProxy>();
        _localizationProxy = Substitute.For<AbpApplicationLocalizationClientProxy>();

        services.Replace(ServiceDescriptor.Transient(_ => _configProxy));
        services.Replace(ServiceDescriptor.Transient(_ => _localizationProxy));
    }

    [Fact]
    public async Task Should_Use_CurrentUICulture_For_Localization_Request()
    {
        var cultureName = "en";

        using (CultureHelper.Use(cultureName))
        {
            var configTcs = new TaskCompletionSource<ApplicationConfigurationDto>();
            _configProxy.GetAsync(Arg.Any<ApplicationConfigurationRequestOptions>()).Returns(configTcs.Task);

            var expectedResources = new Dictionary<string, ApplicationLocalizationResourceDto>
            {
                ["TestResource"] = new()
            };

            _localizationProxy.GetAsync(Arg.Any<ApplicationLocalizationRequestDto>()).Returns(new ApplicationLocalizationDto { Resources = expectedResources });

            var resultTask = _applicationConfigurationClient.GetAsync();

            // Localization request should be fired before config completes (concurrent).
            await _localizationProxy.Received(1).GetAsync(Arg.Is<ApplicationLocalizationRequestDto>(x => x.CultureName == cultureName && x.OnlyDynamics == true));

            // Now let config complete.
            configTcs.SetResult(CreateConfigDto(cultureName));
            var result = await resultTask;

            result.Localization.Resources.ShouldBe(expectedResources);

            await _configProxy.Received(1).GetAsync(Arg.Is<ApplicationConfigurationRequestOptions>(x => x.IncludeLocalizationResources == false));
        }
    }

    [Fact]
    public async Task Should_Refetch_Localization_When_Culture_Differs()
    {
        var currentCulture = "en";
        var serverCulture = "tr";

        using (CultureHelper.Use(currentCulture))
        {
            _configProxy.GetAsync(Arg.Any<ApplicationConfigurationRequestOptions>()).Returns(CreateConfigDto(serverCulture));

            var wrongResources = new Dictionary<string, ApplicationLocalizationResourceDto>();
            var correctResources = new Dictionary<string, ApplicationLocalizationResourceDto>
            {
                ["TestResource"] = new()
            };

            _localizationProxy.GetAsync(Arg.Is<ApplicationLocalizationRequestDto>(x => x.CultureName == currentCulture)).Returns(new ApplicationLocalizationDto { Resources = wrongResources });
            _localizationProxy.GetAsync(Arg.Is<ApplicationLocalizationRequestDto>(x => x.CultureName == serverCulture)).Returns(new ApplicationLocalizationDto { Resources = correctResources });

            var result = await _applicationConfigurationClient.GetAsync();

            result.Localization.Resources.ShouldBe(correctResources);

            await _localizationProxy.Received(1).GetAsync(Arg.Is<ApplicationLocalizationRequestDto>(x => x.CultureName == currentCulture));
            await _localizationProxy.Received(1).GetAsync(Arg.Is<ApplicationLocalizationRequestDto>(x => x.CultureName == serverCulture));
        }
    }

    [Theory]
    [InlineData("tr", "en")]
    [InlineData("en-US", "en")]
    public async Task Should_Retry_Rejected_Culture_With_Server_Culture(string currentCulture, string serverCulture)
    {
        using (CultureHelper.Use(currentCulture))
        {
            _configProxy.GetAsync(Arg.Any<ApplicationConfigurationRequestOptions>()).Returns(CreateConfigDto(serverCulture));

            var resources = new Dictionary<string, ApplicationLocalizationResourceDto>
            {
                ["TestResource"] = new()
            };
            _localizationProxy.GetAsync(Arg.Is<ApplicationLocalizationRequestDto>(x => x.CultureName == currentCulture))
                .Returns(Task.FromException<ApplicationLocalizationDto>(new AbpRemoteCallException
                {
                    HttpStatusCode = (int)HttpStatusCode.BadRequest
                }));
            _localizationProxy.GetAsync(Arg.Is<ApplicationLocalizationRequestDto>(x => x.CultureName == serverCulture))
                .Returns(new ApplicationLocalizationDto { Resources = resources });

            var result = await _applicationConfigurationClient.GetAsync();

            result.Localization.Resources.ShouldBe(resources);
            await _localizationProxy.Received(1).GetAsync(Arg.Is<ApplicationLocalizationRequestDto>(x => x.CultureName == currentCulture));
            await _localizationProxy.Received(1).GetAsync(Arg.Is<ApplicationLocalizationRequestDto>(x => x.CultureName == serverCulture));
        }
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task Should_Not_Retry_Errors_When_Culture_Has_Not_Changed(HttpStatusCode statusCode)
    {
        using (CultureHelper.Use("en"))
        {
            _configProxy.GetAsync(Arg.Any<ApplicationConfigurationRequestOptions>()).Returns(CreateConfigDto("en"));
            var exception = new AbpRemoteCallException { HttpStatusCode = (int)statusCode };
            _localizationProxy.GetAsync(Arg.Any<ApplicationLocalizationRequestDto>())
                .Returns(Task.FromException<ApplicationLocalizationDto>(exception));

            var result = await Should.ThrowAsync<AbpRemoteCallException>(() => _applicationConfigurationClient.GetAsync());

            result.ShouldBeSameAs(exception);
            await _localizationProxy.Received(1).GetAsync(Arg.Any<ApplicationLocalizationRequestDto>());
        }
    }

    [Theory]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task Should_Not_Retry_Unrelated_Errors_When_Cultures_Differ(HttpStatusCode statusCode)
    {
        using (CultureHelper.Use("tr"))
        {
            _configProxy.GetAsync(Arg.Any<ApplicationConfigurationRequestOptions>()).Returns(CreateConfigDto("en"));
            var exception = new AbpRemoteCallException { HttpStatusCode = (int)statusCode };
            _localizationProxy.GetAsync(Arg.Any<ApplicationLocalizationRequestDto>())
                .Returns(Task.FromException<ApplicationLocalizationDto>(exception));

            var result = await Should.ThrowAsync<AbpRemoteCallException>(() => _applicationConfigurationClient.GetAsync());

            result.ShouldBeSameAs(exception);
            await _localizationProxy.Received(1).GetAsync(Arg.Any<ApplicationLocalizationRequestDto>());
        }
    }

    [Fact]
    public async Task Should_Not_Cache_An_Anonymous_Configuration_For_A_Signed_In_User()
    {
        var userId = Guid.NewGuid();

        using (CultureHelper.Use("en"))
        using (SignIn(userId))
        {
            // A call without the user's access token (a Blazor Server circuit after its request ended) is answered anonymously.
            _configProxy.GetAsync(Arg.Any<ApplicationConfigurationRequestOptions>()).Returns(CreateConfigDto("en"));
            _localizationProxy.GetAsync(Arg.Any<ApplicationLocalizationRequestDto>()).Returns(new ApplicationLocalizationDto { Resources = new() });

            (await _applicationConfigurationClient.GetAsync()).CurrentUser.IsAuthenticated.ShouldBeFalse();

            // The next call asks again, and keeps the user's own configuration.
            _configProxy.GetAsync(Arg.Any<ApplicationConfigurationRequestOptions>()).Returns(CreateConfigDto("en", userId));

            (await _applicationConfigurationClient.GetAsync()).CurrentUser.Id.ShouldBe(userId);
            (await _applicationConfigurationClient.GetAsync()).CurrentUser.Id.ShouldBe(userId);

            await _configProxy.Received(2).GetAsync(Arg.Any<ApplicationConfigurationRequestOptions>());
        }
    }

    private IDisposable SignIn(Guid userId)
    {
        return GetRequiredService<ICurrentPrincipalAccessor>().Change(new ClaimsPrincipal(new ClaimsIdentity(
            new[] { new Claim(AbpClaimTypes.UserId, userId.ToString()) }, "Test")));
    }

    private static ApplicationConfigurationDto CreateConfigDto(string cultureName, Guid? userId = null)
    {
        return new ApplicationConfigurationDto
        {
            Localization =
            {
                CurrentCulture = new CurrentCultureDto { Name = cultureName }
            },
            CurrentUser = new CurrentUserDto { IsAuthenticated = userId.HasValue, Id = userId }
        };
    }
}
