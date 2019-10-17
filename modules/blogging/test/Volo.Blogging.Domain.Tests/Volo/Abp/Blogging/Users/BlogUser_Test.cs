using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Shouldly;
using Volo.Abp.Users;
using Volo.Blogging.Users;
using Xunit;

namespace Volo.Blogging
{
    public class BlogUser_Test
    {
        [Fact]
        public void Update()
        {
            var userId = Guid.NewGuid();
            var tenantId = Guid.NewGuid();
            var blogUser = new BlogUser(new UserData(userId, "bob lee", "boblee@volosoft.com", "lee", "bob", true,
                "123456", true, tenantId));
            var userData = new UserData(userId, "lee bob", "leebob@volosoft.com", "bob", "lee", false,
                "654321", false, tenantId);

            blogUser.Update(userData);

            blogUser.EntityEquals(new BlogUser(userData)).ShouldBeTrue();
        }

        [Fact]
        public void Update_User_Id_Must_Equals()
        {
            var userId = Guid.NewGuid();
            var tenantId = Guid.NewGuid();
            var blogUser = new BlogUser(new UserData(userId, "bob lee", "boblee@volosoft.com", "lee", "bob", true,
                "123456", true, tenantId));

            var userData = new UserData(Guid.NewGuid(), "lee bob", "leebob@volosoft.com", "bob", "lee", false,
                "654321", false, tenantId);

            Assert.Throws<ArgumentException>(() => blogUser.Update(userData));
        }

        [Fact]
        public void Update_Tenant_Id_Must_Equals()
        {
            var userId = Guid.NewGuid();
            var tenantId = Guid.NewGuid();
            var blogUser = new BlogUser(new UserData(userId, "bob lee", "boblee@volosoft.com", "lee", "bob", true,
                "123456", true, tenantId));

            var userData = new UserData(userId, "lee bob", "leebob@volosoft.com", "bob", "lee", false,
                "654321", false, Guid.NewGuid());

            Assert.Throws<ArgumentException>(() => blogUser.Update(userData));
        }

        [Fact]
        public void BlogUserEquals()
        {
            var userId = Guid.NewGuid();
            var tenantId = Guid.NewGuid();
            var blogUser = new BlogUser(new UserData(userId, "bob lee", "john@volosoft.com", "lee", "bob", true,
                "123456", true, tenantId));

            var blogUser2= new BlogUser(new UserData(userId, "bob lee", "john@volosoft.com", "lee", "bob", true,
                "123456", true, tenantId));

            blogUser.EntityEquals(blogUser2).ShouldBeTrue();
        }
    }
}
