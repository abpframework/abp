using System.Text;
using Shouldly;
using Volo.Abp.Localization;
using Xunit;

namespace System;

public class StringExtensions_Tests : IDisposable
{
    private readonly IDisposable _cultureScope;

    public StringExtensions_Tests()
    {
        _cultureScope = CultureHelper.Use("en-US");
    }

    [Fact]
    public void EnsureEndsWith_Test()
    {
        //Expected use-cases
        "Test".EnsureEndsWith('!').ShouldBe("Test!");
        "Test!".EnsureEndsWith('!').ShouldBe("Test!");
        @"C:\test\folderName".EnsureEndsWith('\\').ShouldBe(@"C:\test\folderName\");
        @"C:\test\folderName\".EnsureEndsWith('\\').ShouldBe(@"C:\test\folderName\");
        "Sarı".EnsureEndsWith('ı').ShouldBe("Sarı");

        //Case differences
        "TurkeY".EnsureEndsWith('y').ShouldBe("TurkeYy");
    }

    [Fact]
    public void EnsureEndsWith_CultureSpecific_Test()
    {
        using (CultureHelper.Use("tr-TR"))
        {
            "Kırmızı".EnsureEndsWith('I', StringComparison.CurrentCultureIgnoreCase).ShouldBe("Kırmızı");
        }
    }

    [Fact]
    public void EnsureStartsWith_Test()
    {
        //Expected use-cases
        "Test".EnsureStartsWith('~').ShouldBe("~Test");
        "~Test".EnsureStartsWith('~').ShouldBe("~Test");

        //Case differences
        "Turkey".EnsureStartsWith('t').ShouldBe("tTurkey");
    }

    [Fact]
    public void ToPascalCase_Test()
    {
        (null as string).ToPascalCase().ShouldBe(null);
        "helloWorld".ToPascalCase().ShouldBe("HelloWorld");
        "istanbul".ToPascalCase().ShouldBe("Istanbul");
    }

    [Fact]
    public void ToPascalCase_CurrentCulture_Test()
    {
        using (CultureHelper.Use("tr-TR"))
        {
            "istanbul".ToPascalCase(true).ShouldBe("İstanbul");
        }
    }

    [Fact]
    public void ToCamelCase_Test()
    {
        (null as string).ToCamelCase().ShouldBe(null);
        "HelloWorld".ToCamelCase().ShouldBe("helloWorld");
        "Istanbul".ToCamelCase().ShouldBe("istanbul");
    }

    [Fact]
    public void ToKebabCase_Test()
    {
        (null as string).ToKebabCase().ShouldBe(null);
        "helloMoon".ToKebabCase().ShouldBe("hello-moon");
        "HelloWorld".ToKebabCase().ShouldBe("hello-world");
        "HelloIsparta".ToKebabCase().ShouldBe("hello-isparta");
        "ThisIsSampleText".ToKebabCase().ShouldBe("this-is-sample-text");
    }

    [Fact]
    public void ToSnakeCase_Test()
    {
        (null as string).ToSnakeCase().ShouldBe(null);
        "helloMoon".ToSnakeCase().ShouldBe("hello_moon");
        "HelloWorld".ToSnakeCase().ShouldBe("hello_world");
        "HelloIsparta".ToSnakeCase().ShouldBe("hello_isparta");
        "ThisIsSampleText".ToSnakeCase().ShouldBe("this_is_sample_text");
    }

    [Fact]
    public void ToSentenceCase_Test()
    {
        (null as string).ToSentenceCase().ShouldBe(null);
        "HelloWorld".ToSentenceCase().ShouldBe("Hello world");
        "HelloIsparta".ToSentenceCase().ShouldBe("Hello isparta");
        "ThisIsSampleSentence".ToSentenceCase().ShouldBe("This is sample sentence");
        "thisIsSampleSentence".ToSentenceCase().ShouldBe("this is sample sentence");
    }

    [Fact]
    public void Right_Test()
    {
        const string str = "This is a test string";

        str.Right(3).ShouldBe("ing");
        str.Right(0).ShouldBe("");
        str.Right(str.Length).ShouldBe(str);
    }

    [Fact]
    public void Left_Test()
    {
        const string str = "This is a test string";

        str.Left(3).ShouldBe("Thi");
        str.Left(0).ShouldBe("");
        str.Left(str.Length).ShouldBe(str);
    }

    [Fact]
    public void NormalizeLineEndings_Test()
    {
        const string str = "This\r\n is a\r test \n string";
        var normalized = str.NormalizeLineEndings();
        var lines = normalized.SplitToLines();
        lines.Length.ShouldBe(4);
    }

    [Fact]
    public void NthIndexOf_Test()
    {
        const string str = "This is a test string";

        str.NthIndexOf('i', 0).ShouldBe(-1);
        str.NthIndexOf('i', 1).ShouldBe(2);
        str.NthIndexOf('i', 2).ShouldBe(5);
        str.NthIndexOf('i', 3).ShouldBe(18);
        str.NthIndexOf('i', 4).ShouldBe(-1);
    }

    [Fact]
    public void Truncate_Test()
    {
        const string str = "This is a test string";
        const string nullValue = null;

        str.Truncate(7).ShouldBe("This is");
        str.Truncate(0).ShouldBe("");
        str.Truncate(100).ShouldBe(str);

        nullValue.Truncate(5).ShouldBe(null);
    }

    [Fact]
    public void TruncateWithPostFix_Test()
    {
        const string str = "This is a test string";
        const string nullValue = null;

        str.TruncateWithPostfix(3).ShouldBe("...");
        str.TruncateWithPostfix(12).ShouldBe("This is a...");
        str.TruncateWithPostfix(0).ShouldBe("");
        str.TruncateWithPostfix(100).ShouldBe(str);

        nullValue.Truncate(5).ShouldBe(null);

        str.TruncateWithPostfix(3, "~").ShouldBe("Th~");
        str.TruncateWithPostfix(12, "~").ShouldBe("This is a t~");
        str.TruncateWithPostfix(0, "~").ShouldBe("");
        str.TruncateWithPostfix(100, "~").ShouldBe(str);

        nullValue.TruncateWithPostfix(5, "~").ShouldBe(null);
    }

    [Fact]
    public void RemovePostFix_Tests()
    {
        //null case
        (null as string).RemovePostFix("Test").ShouldBeNull();

        //empty case
        string.Empty.RemovePostFix("Test").ShouldBe(string.Empty);

        //Simple case
        "MyTestAppService".RemovePostFix("AppService").ShouldBe("MyTest");
        "MyTestAppService".RemovePostFix("Service").ShouldBe("MyTestApp");

        //Multiple postfix (orders of postfixes are important)
        "MyTestAppService".RemovePostFix("AppService", "Service").ShouldBe("MyTest");
        "MyTestAppService".RemovePostFix("Service", "AppService").ShouldBe("MyTestApp");

        //Ignore case
        "TestString".RemovePostFix(StringComparison.OrdinalIgnoreCase, "string").ShouldBe("Test");

        //Unmatched case
        "MyTestAppService".RemovePostFix("Unmatched").ShouldBe("MyTestAppService");
    }

    [Fact]
    public void RemovePreFix_Tests()
    {
        //null case
        (null as string).RemovePreFix("Test").ShouldBeNull();

        //empty case
        string.Empty.RemovePreFix("Test").ShouldBe(string.Empty);

        "Home.Index".RemovePreFix("NotMatchedPrefix").ShouldBe("Home.Index");
        "Home.About".RemovePreFix("Home.").ShouldBe("About");

        //Ignore case
        "Https://abp.io".RemovePreFix(StringComparison.OrdinalIgnoreCase, "https://").ShouldBe("abp.io");
    }

    [Fact]
    public void ReplaceFirst_Tests()
    {
        "Test string".ReplaceFirst("s", "X").ShouldBe("TeXt string");
        "Test test test".ReplaceFirst("test", "XX").ShouldBe("Test XX test");
        "Test test test".ReplaceFirst("test", "XX", StringComparison.OrdinalIgnoreCase).ShouldBe("XX test test");
    }

    [Fact]
    public void ToEnum_Test()
    {
        "MyValue1".ToEnum<MyEnum>().ShouldBe(MyEnum.MyValue1);
        "MyValue2".ToEnum<MyEnum>().ShouldBe(MyEnum.MyValue2);
    }

    [Theory]
    [InlineData("")]
    [InlineData("MyStringİ")]
    public void GetBytes_Test(string str)
    {
        var bytes = str.GetBytes();
        bytes.ShouldNotBeNull();
        bytes.Length.ShouldBeGreaterThanOrEqualTo(str.Length);
        Encoding.UTF8.GetString(bytes).ShouldBe(str);
    }

    [Theory]
    [InlineData("")]
    [InlineData("MyString")]
    public void GetBytes_With_Encoding_Test(string str)
    {
        var bytes = str.GetBytes(Encoding.ASCII);
        bytes.ShouldNotBeNull();
        bytes.Length.ShouldBeGreaterThanOrEqualTo(str.Length);
        Encoding.ASCII.GetString(bytes).ShouldBe(str);
    }

    [Theory]
    [InlineData("", "D41D8CD98F00B204E9800998ECF8427E")]
    [InlineData("abc", "900150983CD24FB0D6963F7D28E17F72")]
    public void ToMd5_Test(string str, string expected)
    {
        str.ToMd5().ShouldBe(expected);
    }

    [Theory]
    [InlineData("", "cf83e1357eefb8bdf1542850d66d8007d620e4050b5715dc83f4a921d36ce9ce47d0d13c5d85f2b0ff8318d2877eec2f63b931bd47417a81a538327af927da3e")]
    [InlineData("abc", "ddaf35a193617abacc417349ae20413112e6fa4e89a97ea20a9eeee64b55d39a2192992a274fc1a836ba3c23a3feebbd454d4423643ce80e2a9ac94fa54ca49f")]
    public void ToSha512_Test(string str, string expected)
    {
        str.ToSha512().ShouldBe(expected);
    }

    [Theory]
    [InlineData("", "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855")]
    [InlineData("abc", "ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad")]
    public void ToSha256_Test(string str, string expected)
    {
        str.ToSha256().ShouldBe(expected);
    }

    private enum MyEnum
    {
        MyValue1,
        MyValue2
    }

    public void Dispose()
    {
        _cultureScope.Dispose();
    }
}
