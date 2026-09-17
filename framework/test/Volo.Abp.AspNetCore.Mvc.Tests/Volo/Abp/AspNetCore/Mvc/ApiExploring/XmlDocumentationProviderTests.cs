#nullable enable
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using Shouldly;
using Volo.Abp.DependencyInjection;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.ApiExploring;

public class XmlDocumentationProviderTests
{
    // A stub type so we can construct member-name keys that the provider can look up.
    private class StubType { }

    private static XmlDocumentationProvider CreateProvider(string xmlDocBody)
    {
        var xml = $@"<?xml version=""1.0""?>
<doc>
  <members>
    {xmlDocBody}
  </members>
</doc>";
        return new FakeXmlDocumentationProvider(xml);
    }

    private static string StubTypeMemberName(string elementName, string xmlContent)
    {
        var typeName = typeof(StubType).FullName!.Replace('+', '.');
        return $@"<member name=""{elementName}:{typeName}"">
      {xmlContent}
    </member>";
    }

    // Tests for CleanXmlText via GetSummaryAsync(Type)

    [Fact]
    public async Task GetSummary_Returns_PlainText()
    {
        var typeName = typeof(StubType).FullName!.Replace('+', '.');
        var provider = CreateProvider(
            $@"<member name=""T:{typeName}""><summary>A simple summary.</summary></member>");

        var result = await provider.GetSummaryAsync(typeof(StubType));

        result.ShouldBe("A simple summary.");
    }

    [Fact]
    public async Task GetSummary_Expands_SeeCref_To_ShortTypeName()
    {
        var typeName = typeof(StubType).FullName!.Replace('+', '.');
        var provider = CreateProvider(
            $@"<member name=""T:{typeName}""><summary>Returns a <see cref=""T:System.String"" /> value.</summary></member>");

        var result = await provider.GetSummaryAsync(typeof(StubType));

        result.ShouldBe("Returns a String value.");
    }

    [Fact]
    public async Task GetSummary_Expands_SeeCref_NestedType()
    {
        var typeName = typeof(StubType).FullName!.Replace('+', '.');
        var provider = CreateProvider(
            $@"<member name=""T:{typeName}""><summary>See <see cref=""T:System.Collections.Generic.List`1"" /> for details.</summary></member>");

        var result = await provider.GetSummaryAsync(typeof(StubType));

        result.ShouldBe("See List`1 for details.");
    }

    [Fact]
    public async Task GetSummary_Expands_SeeLangword()
    {
        var typeName = typeof(StubType).FullName!.Replace('+', '.');
        var provider = CreateProvider(
            $@"<member name=""T:{typeName}""><summary>Returns <see langword=""null"" /> when not found.</summary></member>");

        var result = await provider.GetSummaryAsync(typeof(StubType));

        result.ShouldBe("Returns null when not found.");
    }

    [Fact]
    public async Task GetSummary_Strips_CodeTag()
    {
        var typeName = typeof(StubType).FullName!.Replace('+', '.');
        var provider = CreateProvider(
            $@"<member name=""T:{typeName}""><summary>Use <c>DoSomething()</c> to start.</summary></member>");

        var result = await provider.GetSummaryAsync(typeof(StubType));

        result.ShouldBe("Use DoSomething() to start.");
    }

    [Fact]
    public async Task GetSummary_Strips_ParaTag()
    {
        var typeName = typeof(StubType).FullName!.Replace('+', '.');
        var provider = CreateProvider(
            $@"<member name=""T:{typeName}""><summary><para>First paragraph.</para></summary></member>");

        var result = await provider.GetSummaryAsync(typeof(StubType));

        result.ShouldBe("First paragraph.");
    }

    [Fact]
    public async Task GetSummary_Collapses_Whitespace()
    {
        var typeName = typeof(StubType).FullName!.Replace('+', '.');
        var provider = CreateProvider(
            $@"<member name=""T:{typeName}""><summary>
              Multiple
              spaces    here.
            </summary></member>");

        var result = await provider.GetSummaryAsync(typeof(StubType));

        result.ShouldBe("Multiple spaces here.");
    }

    [Fact]
    public async Task GetSummary_Returns_Null_When_Member_Not_Found()
    {
        var provider = CreateProvider(string.Empty);

        var result = await provider.GetSummaryAsync(typeof(StubType));

        result.ShouldBeNull();
    }

    [Fact]
    public async Task GetSummary_Returns_Null_When_Summary_Is_Empty()
    {
        var typeName = typeof(StubType).FullName!.Replace('+', '.');
        var provider = CreateProvider(
            $@"<member name=""T:{typeName}""><summary>   </summary></member>");

        var result = await provider.GetSummaryAsync(typeof(StubType));

        result.ShouldBeNull();
    }

    [Fact]
    public async Task GetSummary_Expands_Paramref()
    {
        var typeName = typeof(StubType).FullName!.Replace('+', '.');
        var provider = CreateProvider(
            $@"<member name=""T:{typeName}""><summary>Use the <paramref name=""input"" /> parameter.</summary></member>");

        var result = await provider.GetSummaryAsync(typeof(StubType));

        result.ShouldBe("Use the input parameter.");
    }

    [Fact]
    public async Task GetSummary_Expands_Typeparamref()
    {
        var typeName = typeof(StubType).FullName!.Replace('+', '.');
        var provider = CreateProvider(
            $@"<member name=""T:{typeName}""><summary>Returns <typeparamref name=""T"" /> instance.</summary></member>");

        var result = await provider.GetSummaryAsync(typeof(StubType));

        result.ShouldBe("Returns T instance.");
    }

    [Fact]
    public async Task GetSummary_Expands_Mixed_Tags()
    {
        var typeName = typeof(StubType).FullName!.Replace('+', '.');
        var provider = CreateProvider(
            $@"<member name=""T:{typeName}""><summary>Returns <see cref=""T:System.String"" /> or <see langword=""null"" /> if <paramref name=""key"" /> not found.</summary></member>");

        var result = await provider.GetSummaryAsync(typeof(StubType));

        result.ShouldBe("Returns String or null if key not found.");
    }

    [Fact]
    public async Task GetRemarks_Returns_Null_When_No_Remarks_Element()
    {
        var typeName = typeof(StubType).FullName!.Replace('+', '.');
        var provider = CreateProvider(
            $@"<member name=""T:{typeName}""><summary>Only summary.</summary></member>");

        var result = await provider.GetRemarksAsync(typeof(StubType));

        result.ShouldBeNull();
    }

    [Fact]
    public async Task GetRemarks_Returns_Remarks_Content()
    {
        var typeName = typeof(StubType).FullName!.Replace('+', '.');
        var provider = CreateProvider(
            $@"<member name=""T:{typeName}""><summary>Summary text.</summary><remarks>Some remarks here.</remarks></member>");

        var result = await provider.GetRemarksAsync(typeof(StubType));

        result.ShouldBe("Some remarks here.");
    }

    [Fact]
    public async Task GetSummary_Returns_Null_For_SelfClosing_Summary_Tag()
    {
        var typeName = typeof(StubType).FullName!.Replace('+', '.');
        var provider = CreateProvider(
            $@"<member name=""T:{typeName}""><summary/></member>");

        var result = await provider.GetSummaryAsync(typeof(StubType));

        result.ShouldBeNull();
    }

    [Fact]
    public async Task GetSummary_Expands_SeeCref_Without_TypePrefix()
    {
        var typeName = typeof(StubType).FullName!.Replace('+', '.');
        var provider = CreateProvider(
            $@"<member name=""T:{typeName}""><summary>See <see cref=""M:Foo.Bar.DoWork"" /> for details.</summary></member>");

        var result = await provider.GetSummaryAsync(typeof(StubType));

        result.ShouldBe("See DoWork for details.");
    }

    [Fact]
    public async Task GetSummary_Expands_SeeCref_Property()
    {
        var typeName = typeof(StubType).FullName!.Replace('+', '.');
        var provider = CreateProvider(
            $@"<member name=""T:{typeName}""><summary>See <see cref=""P:Foo.Bar.Name"" /> property.</summary></member>");

        var result = await provider.GetSummaryAsync(typeof(StubType));

        result.ShouldBe("See Name property.");
    }

    [Fact]
    public async Task GetSummary_Strips_Multiple_Xml_Tags()
    {
        var typeName = typeof(StubType).FullName!.Replace('+', '.');
        var provider = CreateProvider(
            $@"<member name=""T:{typeName}""><summary><para>First.</para> <c>code</c> <b>bold</b> end.</summary></member>");

        var result = await provider.GetSummaryAsync(typeof(StubType));

        result.ShouldBe("First. code bold end.");
    }

    // Tests for GetSummaryAsync(MethodInfo) and GetReturnsAsync(MethodInfo)

    private class StubService
    {
        public string GetValue(string key, int count) => key;
        public string NoParams() => string.Empty;
        public string Name { get; set; } = default!;
    }

    [Fact]
    public async Task GetSummary_For_Method_Returns_Summary()
    {
        var typeName = typeof(StubService).FullName!.Replace('+', '.');
        var method = typeof(StubService).GetMethod(nameof(StubService.GetValue))!;
        var provider = CreateProvider(
            $@"<member name=""M:{typeName}.GetValue(System.String,System.Int32)""><summary>Gets a value by key.</summary></member>");

        var result = await provider.GetSummaryAsync(method);

        result.ShouldBe("Gets a value by key.");
    }

    [Fact]
    public async Task GetSummary_For_Method_Without_Parameters_Returns_Summary()
    {
        var typeName = typeof(StubService).FullName!.Replace('+', '.');
        var method = typeof(StubService).GetMethod(nameof(StubService.NoParams))!;
        var provider = CreateProvider(
            $@"<member name=""M:{typeName}.NoParams""><summary>No params method.</summary></member>");

        var result = await provider.GetSummaryAsync(method);

        result.ShouldBe("No params method.");
    }

    [Fact]
    public async Task GetReturns_For_Method_Returns_Content()
    {
        var typeName = typeof(StubService).FullName!.Replace('+', '.');
        var method = typeof(StubService).GetMethod(nameof(StubService.GetValue))!;
        var provider = CreateProvider(
            $@"<member name=""M:{typeName}.GetValue(System.String,System.Int32)""><returns>The resolved value.</returns></member>");

        var result = await provider.GetReturnsAsync(method);

        result.ShouldBe("The resolved value.");
    }

    [Fact]
    public async Task GetReturns_Returns_Null_When_No_Returns_Element()
    {
        var typeName = typeof(StubService).FullName!.Replace('+', '.');
        var method = typeof(StubService).GetMethod(nameof(StubService.GetValue))!;
        var provider = CreateProvider(
            $@"<member name=""M:{typeName}.GetValue(System.String,System.Int32)""><summary>Gets a value.</summary></member>");

        var result = await provider.GetReturnsAsync(method);

        result.ShouldBeNull();
    }

    [Fact]
    public async Task GetParameterSummary_Returns_Content()
    {
        var typeName = typeof(StubService).FullName!.Replace('+', '.');
        var method = typeof(StubService).GetMethod(nameof(StubService.GetValue))!;
        var provider = CreateProvider(
            $@"<member name=""M:{typeName}.GetValue(System.String,System.Int32)""><param name=""key"">The lookup key.</param><param name=""count"">Max results.</param></member>");

        var result = await provider.GetParameterSummaryAsync(method, "key");
        result.ShouldBe("The lookup key.");

        var result2 = await provider.GetParameterSummaryAsync(method, "count");
        result2.ShouldBe("Max results.");
    }

    [Fact]
    public async Task GetParameterSummary_Returns_Null_When_Param_Not_Found()
    {
        var typeName = typeof(StubService).FullName!.Replace('+', '.');
        var method = typeof(StubService).GetMethod(nameof(StubService.GetValue))!;
        var provider = CreateProvider(
            $@"<member name=""M:{typeName}.GetValue(System.String,System.Int32)""><param name=""key"">The key.</param></member>");

        var result = await provider.GetParameterSummaryAsync(method, "nonExistent");

        result.ShouldBeNull();
    }

    [Fact]
    public async Task GetParameterSummary_Returns_Null_When_Member_Not_Found()
    {
        var method = typeof(StubService).GetMethod(nameof(StubService.GetValue))!;
        var provider = CreateProvider(string.Empty);

        var result = await provider.GetParameterSummaryAsync(method, "key");

        result.ShouldBeNull();
    }

    // Tests for GetSummaryAsync(PropertyInfo)

    [Fact]
    public async Task GetSummary_For_Property_Returns_Summary()
    {
        var typeName = typeof(StubService).FullName!.Replace('+', '.');
        var property = typeof(StubService).GetProperty(nameof(StubService.Name))!;
        var provider = CreateProvider(
            $@"<member name=""P:{typeName}.Name""><summary>The name property.</summary></member>");

        var result = await provider.GetSummaryAsync(property);

        result.ShouldBe("The name property.");
    }

    [Fact]
    public async Task GetSummary_For_Property_Returns_Null_When_Not_Found()
    {
        var property = typeof(StubService).GetProperty(nameof(StubService.Name))!;
        var provider = CreateProvider(string.Empty);

        var result = await provider.GetSummaryAsync(property);

        result.ShouldBeNull();
    }

    // Tests for GetRemarksAsync(MethodInfo)

    [Fact]
    public async Task GetRemarks_For_Method_Returns_Content()
    {
        var typeName = typeof(StubService).FullName!.Replace('+', '.');
        var method = typeof(StubService).GetMethod(nameof(StubService.GetValue))!;
        var provider = CreateProvider(
            $@"<member name=""M:{typeName}.GetValue(System.String,System.Int32)""><remarks>Implementation note.</remarks></member>");

        var result = await provider.GetRemarksAsync(method);

        result.ShouldBe("Implementation note.");
    }

    [Fact]
    public async Task GetRemarks_For_Method_Returns_Null_When_Not_Found()
    {
        var typeName = typeof(StubService).FullName!.Replace('+', '.');
        var method = typeof(StubService).GetMethod(nameof(StubService.GetValue))!;
        var provider = CreateProvider(
            $@"<member name=""M:{typeName}.GetValue(System.String,System.Int32)""><summary>Summary only.</summary></member>");

        var result = await provider.GetRemarksAsync(method);

        result.ShouldBeNull();
    }

    /// <summary>
    /// A fake provider that loads XML from an in-memory string instead of the file system.
    /// </summary>
    [DisableConventionalRegistration]
    private sealed class FakeXmlDocumentationProvider : XmlDocumentationProvider
    {
        private readonly XDocument _document;

        public FakeXmlDocumentationProvider(string xml)
        {
            _document = XDocument.Parse(xml);
        }

        protected override Task<XDocument?> LoadXmlDocumentationFromDiskAsync(Assembly assembly)
        {
            return Task.FromResult<XDocument?>(_document);
        }
    }
}
