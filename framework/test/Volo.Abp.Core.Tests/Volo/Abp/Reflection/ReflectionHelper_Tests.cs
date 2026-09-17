using System;
using System.Linq;
using Shouldly;
using Xunit;

namespace Volo.Abp.Reflection;

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

    [Fact]
    public void IsNullable_Test()
    {
        var prop1 = typeof(TestClass).GetProperty(nameof(TestClass.Prop1))!;
        ReflectionHelper.IsNullable(prop1).ShouldBeFalse();

        var prop2 = typeof(TestClass).GetProperty(nameof(TestClass.Prop2))!;
        ReflectionHelper.IsNullable(prop2).ShouldBeTrue();

        var prop3 = typeof(TestClass).GetProperty(nameof(TestClass.Prop3))!;
        ReflectionHelper.IsNullable(prop3).ShouldBeFalse();

        var prop4 = typeof(TestClass).GetProperty(nameof(TestClass.Prop4))!;
        ReflectionHelper.IsNullable(prop4).ShouldBeTrue();

        var prop5 = typeof(TestClass).GetProperty(nameof(TestClass.Prop5))!;
        ReflectionHelper.IsNullable(prop5).ShouldBeFalse();

        var prop6 = typeof(TestClass).GetProperty(nameof(TestClass.Prop6))!;
        ReflectionHelper.IsNullable(prop6).ShouldBeTrue();

        var prop7 = typeof(TestClass).GetProperty(nameof(TestClass.Prop7))!;
        ReflectionHelper.IsNullable(prop7).ShouldBeFalse();

        var prop8 = typeof(TestClass).GetProperty(nameof(TestClass.Prop8))!;
        ReflectionHelper.IsNullable(prop8).ShouldBeTrue();

        var prop9 = typeof(TestClass).GetProperty(nameof(TestClass.Prop9))!;
        ReflectionHelper.IsNullable(prop9).ShouldBeFalse();

        var prop10 = typeof(TestClass).GetProperty(nameof(TestClass.Prop10))!;
        ReflectionHelper.IsNullable(prop10).ShouldBeTrue();

        var prop11 = typeof(TestClass).GetProperty(nameof(TestClass.Prop11))!;
        ReflectionHelper.IsNullable(prop11).ShouldBeFalse();

        var prop12 = typeof(TestClass).GetProperty(nameof(TestClass.Prop12))!;
        ReflectionHelper.IsNullable(prop12).ShouldBeTrue();
    }
}

public class TestClass
{
    public string Prop1 { get; set; } = null!;
    public string? Prop2 { get; set; } = null!;
    public required string Prop3 { get; set; }
    public required string? Prop4 { get; set; }

    public int Prop5 { get; set; }
    public int? Prop6 { get; set; }
    public required int Prop7 { get; set; }
    public required int? Prop8 { get; set; }

    public int[] Prop9 { get; set; } = null!;
    public int[]? Prop10 { get; set; }
    public required int[] Prop11 { get; set; }
    public required int[]? Prop12 { get; set; }
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
