using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace Volo.CmsKit
{
    public class CmsKitFakeCurrentUser : ITransientDependency, ICurrentUser
    {
        private readonly CmsKitTestData cmsKitTestData;

        public CmsKitFakeCurrentUser(CmsKitTestData cmsKitTestData)
        {
            this.cmsKitTestData = cmsKitTestData;
        }

        public bool IsAuthenticated { get; }
        public Guid? Id => cmsKitTestData.User1Id;
        public string UserName => cmsKitTestData.User1UserName;
        public string Name { get; }
        public string SurName { get; }
        public string PhoneNumber { get; }
        public bool PhoneNumberVerified { get; }
        public string Email { get; }
        public bool EmailVerified { get; }
        public Guid? TenantId { get; }
        public string[] Roles { get; }

        public Claim FindClaim(string claimType)
        {
            throw new NotImplementedException();
        }

        public Claim[] FindClaims(string claimType)
        {
            throw new NotImplementedException();
        }

        public Claim[] GetAllClaims()
        {
            throw new NotImplementedException();
        }

        public bool IsInRole(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}
