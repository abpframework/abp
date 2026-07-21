using Shouldly;
using System.Collections.Generic;
using System.Reflection;
using Volo.Abp.Cli.ProjectBuilding.Files;
using Xunit;

namespace Volo.Abp.Cli.ProjectBuilding.Building.Steps;

public class SolutionRenamer_Tests
{
    [Theory]
    [InlineData("Demo", "demo")]
    [InlineData("MyCompany", "myCompany")]
    [InlineData("Acme", "acme")]
    [InlineData("ABC", "aBC")]
    public void ToCamelCaseWithNamespace_Should_Handle_Single_Segment_Names(string input, string expected)
    {
        // Act
        var result = InvokeToCamelCaseWithNamespace(input);

        // Assert
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData("Demo.App", "demo.app")]
    [InlineData("MyCompany.MyProject", "myCompany.myProject")]
    [InlineData("Acme.Bookstore", "acme.bookstore")]
    [InlineData("ABC.XYZ", "aBC.xYZ")]
    public void ToCamelCaseWithNamespace_Should_Handle_Two_Segment_Names(string input, string expected)
    {
        // Act
        var result = InvokeToCamelCaseWithNamespace(input);

        // Assert
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData("Demo.App.QoL", "demo.app.qoL")]
    [InlineData("MyCompany.MyProject.Module", "myCompany.myProject.module")]
    [InlineData("Acme.Bookstore.Application", "acme.bookstore.application")]
    [InlineData("A.B.C.D", "a.b.c.d")]
    public void ToCamelCaseWithNamespace_Should_Handle_Multi_Segment_Names(string input, string expected)
    {
        // Act
        var result = InvokeToCamelCaseWithNamespace(input);

        // Assert
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("A", "a")]
    [InlineData(".", ".")]
    [InlineData("...", "...")]
    public void ToCamelCaseWithNamespace_Should_Handle_Edge_Cases(string input, string expected)
    {
        // Act
        var result = InvokeToCamelCaseWithNamespace(input);

        // Assert
        result.ShouldBe(expected);
    }

    [Fact]
    public void ToCamelCaseWithNamespace_Should_Throw_On_Null_Input()
    {
        // Act & Assert
        var exception = Should.Throw<System.Reflection.TargetInvocationException>(() => InvokeToCamelCaseWithNamespace(null));
        exception.InnerException.ShouldBeOfType<System.NullReferenceException>();
    }

    /// <summary>
    /// Helper method to invoke the private static ToCamelCaseWithNamespace method using reflection
    /// </summary>
    private static string InvokeToCamelCaseWithNamespace(string input)
    {
        var type = typeof(SolutionRenamer);
        var method = type.GetMethod("ToCamelCaseWithNamespace", BindingFlags.NonPublic | BindingFlags.Static);
        
        method.ShouldNotBeNull("ToCamelCaseWithNamespace method should exist");
        
        return (string)method.Invoke(null, new object[] { input });
    }
}
