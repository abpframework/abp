using System;
using System.Linq;
using Shouldly;
using Xunit;

namespace Volo.Abp.Reflection
{
    public class ReflectionHelper_Tests
    {
        //TODO: ...

        class GetValueByPathTestClass
        {
            public string Name { get; set; }

            public int Count { get; set; }

            public DateTime Time { get; set; }

            public GetValueByPathTestChildrenClass Children { get; set; }
        }

        class GetValueByPathTestChildrenClass
        {
            public string Name { get; set; }

            public int Count { get; set; }
        }

        [Fact]
        public void GetValueByPath_Test()
        {
            var value = new GetValueByPathTestClass
            {
                Name = "test",
                Count = 8,
                Time = DateTime.Parse("2020-01-01"),
                Children = new GetValueByPathTestChildrenClass
                {
                    Name = "test-children",
                    Count = 9,
                }
            };

            ReflectionHelper.GetValueByPath(value, value.GetType(), "Name").ShouldBe("test");
            ReflectionHelper.GetValueByPath(value, value.GetType(), "Volo.Abp.Reflection.ReflectionHelper_Tests+GetValueByPathTestClass.Name").ShouldBe("test");
            ReflectionHelper.GetValueByPath(value, value.GetType(), "Count").ShouldBe(8);
            ReflectionHelper.GetValueByPath(value, value.GetType(), "Time").ShouldBe(DateTime.Parse("2020-01-01"));
            ReflectionHelper.GetValueByPath(value, value.GetType(), "Children.Name").ShouldBe("test-children");
            ReflectionHelper.GetValueByPath(value, value.GetType(), "Children.Count").ShouldBe(9);
            ReflectionHelper.GetValueByPath(value, value.GetType(), "Volo.Abp.Reflection.ReflectionHelper_Tests+GetValueByPathTestClass.Children.Name").ShouldBe("test-children");

            ReflectionHelper.GetValueByPath(value, value.GetType(), "Children.NotExist").ShouldBeNull();
            ReflectionHelper.GetValueByPath(value, value.GetType(), "NotExist").ShouldBeNull();
        }

        [Fact]
        public void GetPublicConstantsRecursively_Test()
        {
            var constants = ReflectionHelper.GetPublicConstantsRecursively(typeof(BaseRole));

            constants.ShouldNotBeEmpty();
            constants.Length.ShouldBe(1);
            constants.ShouldContain(x => x == "DefaultBaseRoleName");
        }

        [Fact]
        public void GetPublicConstantsRecursively_Inherit_Test()
        {
            var constants = ReflectionHelper.GetPublicConstantsRecursively(typeof(Roles));

            constants.ShouldNotBeEmpty();
            constants.Length.ShouldBe(2);
            constants.ShouldContain(x => x == "DefaultBaseRoleName");
            constants.ShouldContain(x => x == "DefaultRoleName");
        }


        [Fact]
        public void GetPublicConstantsRecursively_NestedTypes_Test()
        {
            var constants = ReflectionHelper.GetPublicConstantsRecursively(typeof(IdentityPermissions));

            constants.ShouldNotBeEmpty();
            constants.Except(IdentityPermissions.GetAll()).Count().ShouldBe(0);
        }
    }


    public class BaseRole
    {
        public const string BaseRoleName = "DefaultBaseRoleName";
    }

    public class Roles : BaseRole
    {
        public const string RoleName = "DefaultRoleName";
    }

    public static class IdentityPermissions
    {
        public const string GroupName = "AbpIdentity";

        public static class Roles
        {
            public const string Default = GroupName + ".Roles";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManagePermissions = Default + ".ManagePermissions";
        }

        public static class Users
        {
            public const string Default = GroupName + ".Users";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManagePermissions = Default + ".ManagePermissions";
        }

        public static class UserLookup
        {
            public const string Default = GroupName + ".UserLookup";
        }

        public static string[] GetAll()
        {
            return new[]
            {
                GroupName,
                Roles.Default,
                Roles.Create,
                Roles.Update,
                Roles.Delete,
                Roles.ManagePermissions,
                Users.Default,
                Users.Create,
                Users.Update,
                Users.Delete,
                Users.ManagePermissions,
                UserLookup.Default
            };
        }
    }

}
