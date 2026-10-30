using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.Users;
using Volo.Docs.Admin.Projects;
using Volo.Docs.Projects;
using Xunit;

namespace Volo.Docs
{
    public class ProjectAdminAppServiceAuthorization_Tests : DocsAdminApplicationTestBase
    {
        protected override void AfterAddApplication(IServiceCollection services)
        {
            base.AfterAddApplication(services);
            services.Replace(ServiceDescriptor.Transient<IAuthorizationService, AbpAuthorizationService>());
            services.Replace(ServiceDescriptor.Transient<IAbpAuthorizationService, AbpAuthorizationService>());
            services.Replace(ServiceDescriptor.Transient<IMethodInvocationAuthorizationService, MethodInvocationAuthorizationService>());
            var permissionChecker = Substitute.For<IPermissionChecker>();
            permissionChecker.IsGrantedAsync(Arg.Any<ClaimsPrincipal>(), Arg.Any<string>()).Returns(false);
            services.Replace(ServiceDescriptor.Singleton(permissionChecker));
        }

        [Theory]
        [InlineData("GetAsync")]
        [InlineData("GetListAsync")]
        [InlineData("CreateAsync")]
        [InlineData("UpdateAsync")]
        public async Task Should_Deny_Project_Responses_Without_Permission(string method)
        {
            GetRequiredService<IMethodInvocationAuthorizationService>().ShouldBeOfType<MethodInvocationAuthorizationService>();
            GetRequiredService<ICurrentUser>().IsAuthenticated.ShouldBeFalse();
            var service = GetRequiredService<IProjectAdminAppService>();
            var id = GetRequiredService<DocsTestData>().ProjectId;

            Task InvokeAsync()
            {
                return method switch
                {
                    "GetAsync" => service.GetAsync(id),
                    "GetListAsync" => service.GetListAsync(new PagedAndSortedResultRequestDto()),
                    "CreateAsync" => service.CreateAsync(new CreateProjectDto
                    {
                        Name = "Denied project", ShortName = "denied-project", Format = "md",
                        DefaultDocumentName = "CLI", NavigationDocumentName = "docs-nav.json",
                        ParametersDocumentName = "docs-params.json", DocumentStoreType = "GitHub",
                        ExtraProperties = new Dictionary<string, object>()
                    }),
                    "UpdateAsync" => service.UpdateAsync(id, new UpdateProjectDto
                    {
                        Name = "Denied update", Format = "md", DefaultDocumentName = "CLI",
                        NavigationDocumentName = "docs-nav.json", ParametersDocumentName = "docs-params.json",
                        ExtraProperties = new Dictionary<string, object>()
                    }),
                    _ => throw new ArgumentOutOfRangeException(nameof(method))
                };
            }

            await Should.ThrowAsync<AbpAuthorizationException>(InvokeAsync);
            var repository = GetRequiredService<IProjectRepository>();
            var project = await repository.GetAsync(id);
            project.Name.ShouldBe("ABP vNext");
            project.GetProperty<string>("GitHubAccessToken").ShouldBe("123456");
            (await repository.GetListAsync()).Count.ShouldBe(1);
        }
    }
}
