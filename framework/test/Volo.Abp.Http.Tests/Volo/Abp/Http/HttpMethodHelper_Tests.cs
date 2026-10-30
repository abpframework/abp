using System;
using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace Volo.Abp.Http;

public class HttpMethodHelper_Tests
{
    private static readonly Dictionary<string, Func<string?, bool>> Predicates = new()
    {
        [HttpMethodHelper.Get] = HttpMethodHelper.IsGet,
        [HttpMethodHelper.Post] = HttpMethodHelper.IsPost,
        [HttpMethodHelper.Put] = HttpMethodHelper.IsPut,
        [HttpMethodHelper.Delete] = HttpMethodHelper.IsDelete,
        [HttpMethodHelper.Patch] = HttpMethodHelper.IsPatch,
        [HttpMethodHelper.Head] = HttpMethodHelper.IsHead,
        [HttpMethodHelper.Options] = HttpMethodHelper.IsOptions,
        [HttpMethodHelper.Trace] = HttpMethodHelper.IsTrace,
        [HttpMethodHelper.Query] = HttpMethodHelper.IsQuery
    };

    [Theory]
    [InlineData(HttpMethodHelper.Get)]
    [InlineData(HttpMethodHelper.Post)]
    [InlineData(HttpMethodHelper.Put)]
    [InlineData(HttpMethodHelper.Delete)]
    [InlineData(HttpMethodHelper.Patch)]
    [InlineData(HttpMethodHelper.Head)]
    [InlineData(HttpMethodHelper.Options)]
    [InlineData(HttpMethodHelper.Trace)]
    [InlineData(HttpMethodHelper.Query)]
    public void Is_Predicates_Should_Match_Only_Their_Own_Verb_Ignoring_Case(string verb)
    {
        foreach (var (name, predicate) in Predicates)
        {
            var shouldMatch = name == verb;
            predicate(verb).ShouldBe(shouldMatch);
            predicate(verb.ToLowerInvariant()).ShouldBe(shouldMatch);
        }
    }

    [Fact]
    public void Is_Predicates_Should_Return_False_For_Null()
    {
        foreach (var predicate in Predicates.Values)
        {
            predicate(null).ShouldBeFalse();
        }
    }

    [Theory]
    [InlineData("QUERY", true)]
    [InlineData("query", true)]
    [InlineData("Query", true)]
    [InlineData("GET", false)]
    [InlineData("POST", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsQuery_Should_Match_Query_Method_Ignoring_Case(string? httpMethod, bool expected)
    {
        HttpMethodHelper.IsQuery(httpMethod).ShouldBe(expected);
    }

    [Fact]
    public void ConvertToHttpMethod_Should_Support_Query()
    {
        HttpMethodHelper.ConvertToHttpMethod("QUERY").Method.ShouldBe("QUERY");
        HttpMethodHelper.ConvertToHttpMethod("query").Method.ShouldBe("QUERY");
    }

    [Theory]
    [InlineData("GET")]
    [InlineData("POST")]
    [InlineData("PUT")]
    [InlineData("DELETE")]
    [InlineData("PATCH")]
    [InlineData("QUERY")]
    public void ConvertToHttpMethod_Should_Not_Throw_For_Known_Methods(string httpMethod)
    {
        Should.NotThrow(() => HttpMethodHelper.ConvertToHttpMethod(httpMethod));
    }

    [Fact]
    public void ConvertToHttpMethod_Should_Throw_For_Unknown_Method()
    {
        Should.Throw<AbpException>(() => HttpMethodHelper.ConvertToHttpMethod("UNKNOWN"));
    }

    [Theory]
    [InlineData("GetFooAsync", "GET")]
    [InlineData("GetListAsync", "GET")]
    [InlineData("CreateFooAsync", "POST")]
    [InlineData("UpdateFooAsync", "PUT")]
    [InlineData("DeleteFooAsync", "DELETE")]
    [InlineData("PatchFooAsync", "PATCH")]
    [InlineData("DoSomethingAsync", "POST")]
    // QUERY is intentionally NOT a naming convention: an action must opt in explicitly
    // with [AcceptVerbs("QUERY")]. A method named Query* still maps to the default verb.
    [InlineData("QueryFooAsync", "POST")]
    public void GetConventionalVerbForMethodName_Should_Not_Map_Query_By_Name(string methodName, string expectedVerb)
    {
        HttpMethodHelper.GetConventionalVerbForMethodName(methodName).ShouldBe(expectedVerb);
    }
}
