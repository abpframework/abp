using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Volo.Abp.Users;

namespace Volo.Abp.PermissionManagement
{
    public class AbpPermissionManagementApplicationTestBase : PermissionManagementTestBase<PermissionManagementApplicationTestModule>
    {
        protected Guid? CurrentUserId { get; set; }

        protected AbpPermissionManagementApplicationTestBase()
        {
            CurrentUserId = Guid.NewGuid();
        }
        protected override void AfterAddApplication(IServiceCollection services)
        {
            var currentUser = Substitute.For<ICurrentUser>();
            //currentUser.Id.Returns(ci => CurrentUserId);
            currentUser.IsAuthenticated.Returns(true);

            services.AddSingleton(currentUser);
        }
    }
}
