using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;

namespace AuthServer.Host.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentTenant _currentTenant;
        private readonly IdentityUserManager _identityUserManager;

        public TestController(ICurrentUser currentUser, IdentityUserManager identityUserManager, ICurrentTenant currentTenant)
        {
            _currentUser = currentUser;
            _identityUserManager = identityUserManager;
            _currentTenant = currentTenant;
        }

        [Authorize]
        [HttpGet("info")]
        public string Info()
        {
            return _currentUser.GetAllClaims().Select(c => $"{c.Type}={c.Value}").JoinAsString(" <br /> ");
        }

        [HttpGet("getUser")]
        public async Task<IdentityUser> GetUser(string userid)
        {
            return await _identityUserManager.FindByIdAsync(userid);
        }
    }
}