using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using Shouldly;
using Xunit;

namespace Volo.Abp.AgentTools;

public class AgentToolArgumentRedactorTests
{
    private static string Redact(object? value, params string[] additionalNames) =>
        AgentToolArgumentRedactor.ToRedactedJson(
            new Dictionary<string, object?> { ["input"] = value },
            new JsonSerializerOptions(),
            name => AgentToolArgumentRedactor.IsSensitiveName(name, additionalNames));

    [Theory]
    [InlineData("Authorization")]
    [InlineData("Cookie")]
    [InlineData("Set-Cookie")]
    [InlineData("Bearer")]
    [InlineData("SessionId")]
    [InlineData("session_key")]
    [InlineData("accessToken")]
    [InlineData("secret")]
    [InlineData("Password")]
    [InlineData("passwd")]
    [InlineData("PWD")]
    [InlineData("passphrase")]
    [InlineData("Key_Passphrase")]
    [InlineData("accessKey")]
    [InlineData("access_key")]
    [InlineData("AccessKeyId")]
    [InlineData("apiKey")]
    [InlineData("api_key")]
    [InlineData("Api-Key")]
    [InlineData("privateKey")]
    [InlineData("private_key")]
    [InlineData("credential")]
    [InlineData("credentials")]
    [InlineData("ConnectionString")]
    [InlineData("connection_string")]
    [InlineData("clientSecret")]
    [InlineData("Client-Secret")]
    [InlineData("client_secret")]
    public void Every_Listed_Name_Is_Sensitive_In_Any_Case_And_Separator_Variant(string name)
    {
        AgentToolArgumentRedactor.IsSensitiveName(name).ShouldBeTrue();
        AgentToolArgumentRedactor.ToRedactedJson(
            new Dictionary<string, object?> { [name] = "v", ["path"] = "a.txt" }, new JsonSerializerOptions())
            .ShouldBe($"{{\"{name}\":\"***\",\"path\":\"a.txt\"}}");
    }

    [Theory]
    [InlineData("path")]
    [InlineData("userName")]
    [InlineData("name")]
    [InlineData("author")]
    public void Ordinary_Names_Are_Not_Sensitive(string name)
    {
        AgentToolArgumentRedactor.IsSensitiveName(name).ShouldBeFalse();
    }

    [Fact]
    public void Nested_Objects_And_Arrays_Of_Objects_Are_Redacted()
    {
        Redact(new
        {
            user = "bob",
            profile = new { email = "b@x.test", auth = new { Password = "p" } },
            items = new object[] { new { id = 1, apiKey = "k" }, new { id = 2, headers = new[] { new { cookie = "c" } } } }
        }).ShouldBe(
            "{\"input\":{\"user\":\"bob\",\"profile\":{\"email\":\"b@x.test\",\"auth\":{\"Password\":\"***\"}}," +
            "\"items\":[{\"id\":1,\"apiKey\":\"***\"},{\"id\":2,\"headers\":[{\"cookie\":\"***\"}]}]}}");
    }

    [Fact]
    public void An_Object_Or_Array_Valued_Sensitive_Property_Becomes_A_Single_Mask()
    {
        Redact(new { credentials = new { user = "u", pass = "p" }, tokens = new[] { "a", "b" }, keep = 1 })
            .ShouldBe("{\"input\":{\"credentials\":\"***\",\"tokens\":\"***\",\"keep\":1}}");
    }

    [Theory]
    [InlineData("""{"name":"a","name":"b"}""", """{"name":"a","name":"b"}""")]
    [InlineData("""{"name":"a","Name":"b"}""", """{"name":"a","Name":"b"}""")]
    [InlineData("""{"password":"one","id":1,"password":"two"}""", """{"password":"***","id":1,"password":"***"}""")]
    [InlineData("""{"password":"one","PASSWORD":{"a":1,"a":2}}""", """{"password":"***","PASSWORD":"***"}""")]
    [InlineData("""{"items":[{"id":1,"id":2,"token":"t","Token":null}]}""", """{"items":[{"id":1,"id":2,"token":"***","Token":"***"}]}""")]
    [InlineData("""{"a":{"b":1,"b":{"secret":"s","secret":"t"}}}""", """{"a":{"b":1,"b":{"secret":"***","secret":"***"}}}""")]
    public void Duplicate_Names_Are_All_Kept_And_Redacted(string input, string expected)
    {
        foreach (var options in new[] { new JsonSerializerOptions(), new JsonSerializerOptions(JsonSerializerDefaults.Web) })
        {
            AgentToolArgumentRedactor.ToRedactedJson(
                new Dictionary<string, object?> { ["input"] = JsonSerializer.Deserialize<JsonElement>(input) }, options)
                .ShouldBe("{\"input\":" + expected + "}");
        }
    }

    [Fact]
    public void Argument_Names_That_Differ_By_Case_Are_All_Kept_And_Redacted()
    {
        AgentToolArgumentRedactor.ToRedactedJson(
            new Dictionary<string, object?> { ["name"] = "a", ["Name"] = null, ["secret"] = "s", ["Secret"] = 2 },
            new JsonSerializerOptions(JsonSerializerDefaults.Web))
            .ShouldBe("{\"name\":\"a\",\"Name\":null,\"secret\":\"***\",\"Secret\":\"***\"}");
    }

    [Fact]
    public void Values_Are_Written_As_They_Are()
    {
        Redact(JsonSerializer.Deserialize<JsonElement>("""{"n":1.50,"big":12345678901234567890,"t":true,"z":null,"s":"a\"<\u00e7","e":{},"l":[]}"""))
            .ShouldBe("""{"input":{"n":1.50,"big":12345678901234567890,"t":true,"z":null,"s":"a\u0022\u003C\u00E7","e":{},"l":[]}}""");
        AgentToolArgumentRedactor.ToRedactedJson(new Dictionary<string, object?>(), new JsonSerializerOptions()).ShouldBe("{}");
    }

    [Fact]
    public void Names_Added_Through_The_Option_Are_Redacted_Like_The_Built_In_Ones()
    {
        Redact(new { ssn = "1", nested = new { Tax_ID = "2" }, other = "3" }, "SSN", "tax-id")
            .ShouldBe("{\"input\":{\"ssn\":\"***\",\"nested\":{\"Tax_ID\":\"***\"},\"other\":\"3\"}}");

        // Without the added names they pass through; an empty added name matches nothing.
        Redact(new { ssn = "1" }).ShouldBe("{\"input\":{\"ssn\":\"1\"}}");
        Redact(new { ssn = "1" }, "", "_").ShouldBe("{\"input\":{\"ssn\":\"1\"}}");
    }

    /// <summary>The recorded text of a string argument.</summary>
    private static string RedactText(string text, params string[] additionalNames) =>
        JsonSerializer.Deserialize<JsonElement>(Redact(text, additionalNames)).GetProperty("input").GetString()!;

    /// <summary>The text in the "body" string of an object, whose JSON is in the "body" string of another one, and so on.</summary>
    private static string NestInStrings(string text, int strings)
    {
        var options = new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
        return Enumerable.Range(0, strings).Aggregate(text, (inner, _) => JsonSerializer.Serialize(new { body = inner }, options));
    }

    private static string NestInArrays(string json, int arrays) => new string('[', arrays) + json + new string(']', arrays);

    [Theory]
    [InlineData("""{"password":"p","id":1}""", """{"password":"***","id":1}""")]
    [InlineData("""[{"token":"t"},{"id":2},"a",null]""", """[{"token":"***"},{"id":2},"a",null]""")]
    [InlineData("""{"a":{"b":[{"secret":{"x":1}}]}}""", """{"a":{"b":[{"secret":"***"}]}}""")]
    [InlineData(" \r\n\t{\"pwd\":1}\n ", """{"pwd":"***"}""")]
    [InlineData("{/* a */\"password\":\"p\", // b\n\"ids\":[1,2,],}", """{"password":"***","ids":[1,2]}""")]
    [InlineData("""{"password":"1","id":1,"id":2,"Password":"2"}""", """{"password":"***","id":1,"id":2,"Password":"***"}""")]
    [InlineData("""{"pass\u0077ord":"p"}""", """{"password":"***"}""")]
    public void A_String_That_Holds_A_Json_Object_Or_Array_Is_Redacted_Inside(string text, string expected)
    {
        RedactText(text).ShouldBe(expected);
        Redact(new { body = text, list = new[] { text } })
            .ShouldBe(JsonSerializer.Serialize(new { input = new { body = expected, list = new[] { expected } } }));
    }

    [Fact]
    public void Other_Values_In_A_Redacted_String_Are_Written_As_They_Are()
    {
        RedactText("""{"secret":1,"n":1.50,"big":12345678901234567890,"t":true,"z":null,"s":"a\"<\u00e7ğ'","ş":{},"l":[]}""")
            .ShouldBe("""{"secret":"***","n":1.50,"big":12345678901234567890,"t":true,"z":null,"s":"a\"<\u00e7ğ'","ş":{},"l":[]}""");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("text")]
    [InlineData("123")]
    [InlineData("null")]
    [InlineData("{")]
    [InlineData("{}")]
    [InlineData("[]")]
    [InlineData("[ 1, 2, ]")]
    [InlineData("{not json}")]
    [InlineData("[note] text [/note]")]
    [InlineData("{\"a\":1}\n{\"a\":2}")]
    [InlineData("\"{\\\"password\\\":1}\"")] // a JSON string literal
    [InlineData(" { \"id\" : 1 , /* a */ \"name\" : \"\\u00e7\", \"id\" : 2 } ")]
    [InlineData("{ \"body\" : \"{ \\\"id\\\" : 1 }\" }")]
    public void A_String_Without_Anything_To_Redact_Stays_As_It_Is(string text)
    {
        RedactText(text).ShouldBe(text);
    }

    [Fact]
    public void A_Sensitive_Name_With_A_Json_String_Becomes_A_Single_Mask()
    {
        Redact(new { password = "{\"id\":1}" }).ShouldBe("{\"input\":{\"password\":\"***\"}}");
        RedactText("""{"token":"{\"id\":1}"}""").ShouldBe("""{"token":"***"}""");
    }

    [Fact]
    public void Names_Added_Through_The_Option_Are_Redacted_Inside_Strings()
    {
        RedactText("""{"ssn":"1","id":2}""", "SSN").ShouldBe("""{"ssn":"***","id":2}""");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(3)]
    public void Strings_Inside_Strings_Are_Redacted(int nested)
    {
        RedactText(NestInStrings("""{"password":"p"}""", nested)).ShouldBe(NestInStrings("""{"password":"***"}""", nested));
    }

    [Theory]
    [InlineData("""{"password":"p"}""", "***")]
    [InlineData("""{"id":1}""", "***")]
    [InlineData("text", "text")]
    public void A_String_Inside_Too_Many_Strings_Is_Masked_When_It_Looks_Like_Json(string text, string expected)
    {
        RedactText(NestInStrings(text, 4)).ShouldBe(NestInStrings(expected, 4));
    }

    [Fact]
    public void A_String_Over_The_Length_Limit_Is_Masked_When_It_Looks_Like_Json()
    {
        const int limit = 64 * 1024;
        var fits = "{\"password\":\"" + new string('p', limit - 15) + "\"}";
        fits.Length.ShouldBe(limit);

        RedactText(fits).ShouldBe("""{"password":"***"}""");
        RedactText(" " + fits + " ").ShouldBe("""{"password":"***"}""");
        RedactText(fits.Replace("\"}", "p\"}")).ShouldBe("***");
        RedactText("[note]" + new string(' ', limit) + "[/note]").ShouldBe("***");

        var text = "note" + new string(' ', limit) + "{}";
        RedactText(text).ShouldBe(text);
    }

    [Fact]
    public void A_String_Over_The_Json_Depth_Limit_Is_Masked()
    {
        RedactText(NestInArrays("""{"password":"p"}""", 63)).ShouldBe(NestInArrays("""{"password":"***"}""", 63));
        RedactText(NestInArrays("""{"password":"p"}""", 64)).ShouldBe("***");
        RedactText(NestInArrays("", 64)).ShouldBe(NestInArrays("", 64));
        RedactText(NestInArrays("", 65)).ShouldBe("***");
        RedactText(NestInArrays("", 32 * 1024)).ShouldBe("***");
        RedactText(string.Concat(Enumerable.Repeat("{\"a\":", 9_000)) + "1" + new string('}', 9_000)).ShouldBe("***");

        // Not JSON at all.
        var text = NestInArrays("x", 1_000);
        RedactText(text).ShouldBe(text);
    }

    [Theory]
    [InlineData("""{"password":"p","s":"\ud800"}""")]
    [InlineData("""{"\ud800":1,"password":"p"}""")]
    [InlineData("""["\udc00"]""")]
    public void A_String_With_A_Json_String_That_Is_No_Text_Is_Masked(string text)
    {
        RedactText(text).ShouldBe("***");
    }

    [Fact]
    public void Strings_That_Only_Look_Like_Json_Are_Masked_When_There_Are_Too_Many()
    {
        var texts = Enumerable.Repeat("[x]", 100_000).Append("text").Append("""{"id":1}""").ToArray();

        var recorded = JsonSerializer.Deserialize<string[]>(
            JsonSerializer.Deserialize<JsonElement>(Redact(texts)).GetProperty("input").GetRawText())!;

        recorded.Length.ShouldBe(texts.Length);
        recorded.Take(1_000).ShouldAllBe(t => t == "[x]");
        recorded.Skip(1_000).Take(99_000).ShouldAllBe(t => t == "***");
        recorded.TakeLast(2).ShouldBe(["text", "***"]);

        // Valid ones have no limit.
        var valid = Enumerable.Repeat("""{"password":"p"}""", 10_000).ToArray();
        Redact(valid).ShouldBe(JsonSerializer.Serialize(new { input = Enumerable.Repeat("""{"password":"***"}""", 10_000) }));
    }
}
