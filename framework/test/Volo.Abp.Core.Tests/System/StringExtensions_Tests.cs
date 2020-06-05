using System.Text;
using Shouldly;
using Volo.Abp.Localization;
using Xunit;

namespace System
{
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
            (null as string).RemovePreFix("Test").ShouldBeNull();

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
            "Home.Index".RemovePreFix("NotMatchedPostfix").ShouldBe("Home.Index");
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
}