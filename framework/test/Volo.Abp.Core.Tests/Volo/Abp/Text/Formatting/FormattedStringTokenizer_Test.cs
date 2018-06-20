using System;
using Shouldly;
using System.Collections.Generic;
using Xunit;

namespace Volo.Abp.Text.Formatting
{
    public class FormattedStringTokenizer_Test
    {
        [Fact]
        public void Should_Throw_FormatException_For_Invalid_Format()
        {
            Assert.Throws<FormatException>(() => new FormatStringTokenizer().Tokenize("a sample { wrong format"));
            Assert.Throws<FormatException>(() => new FormatStringTokenizer().Tokenize("a sample {0{1}} wrong format"));
            Assert.Throws<FormatException>(() => new FormatStringTokenizer().Tokenize("} wrong format"));
            Assert.Throws<FormatException>(() => new FormatStringTokenizer().Tokenize("wrong {} format"));
        }

        [Fact]
        public void Should_Tokenize_For_Valid_Format()
        {
            TokenizeTest("");
            TokenizeTest("a sample {0} value", "a sample ", "{0}", " value");
            TokenizeTest("{0} is {name} at this {1}.", "{0}", " is ", "{name}", " at this ", "{1}", ".");
        }

        private void TokenizeTest(string format, params string[] expectedTokens)
        {
            var actualTokens = new FormatStringTokenizer().Tokenize(format);
            if (expectedTokens.IsNullOrEmpty())
            {
                actualTokens.Count.ShouldBe(0);
                return;
            }

            actualTokens.Count.ShouldBe(expectedTokens.Length);

            for (var i = 0; i < actualTokens.Count; i++)
            {
                var actualToken = actualTokens[i];
                var expectedToken = expectedTokens[i];

                actualToken.Text.ShouldBe(expectedToken.Trim('{', '}'));

                if (expectedToken.StartsWith("{") && expectedToken.EndsWith("}"))
                {
                    actualToken.Type.ShouldBe(FormatStringTokenType.DynamicValue);
                }
                else
                {
                    actualToken.Type.ShouldBe(FormatStringTokenType.ConstantText);
                }
            }
        }
    }
}
