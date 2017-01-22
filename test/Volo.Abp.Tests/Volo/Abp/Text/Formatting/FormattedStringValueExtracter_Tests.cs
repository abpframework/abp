using Shouldly;
using Xunit;

namespace Volo.Abp.Text.Formatting
{
    public class FormattedStringValueExtracter_Tests
    {
        [Fact]
        public void Test_Matched()
        {
            Test_Matched(
                "My name is Neo.",
                "My name is {0}.",
                new NameValue("0", "Neo")
                );

            Test_Matched(
                "User halil does not exist.",
                "User {0} does not exist.",
                new NameValue("0", "halil")
                );

            Test_Matched(
                "abp.io",
                "{domain}",
                new NameValue("domain", "abp.io")
            );
        }

        [Fact]
        public void Test_Not_Matched()
        {
            Test_Not_Matched(
                "My name is Neo.",
                "My name is Marry."
                );

            Test_Not_Matched(
                "Role {0} does not exist.",
                "User name {0} is invalid, can only contain letters or digits."
                );

            Test_Not_Matched(
                "{0} cannot be null or empty.",
                "Incorrect password."
                );

            Test_Not_Matched(
                "Incorrect password.",
                "{0} cannot be null or empty."
                );
        }

        [Fact]
        public void IsMatch_Test()
        {
            string[] values;
            FormattedStringValueExtracter.IsMatch("User halil does not exist.", "User {0} does not exist.", out values).ShouldBe(true);
            values[0].ShouldBe("halil");
        }

        private static void Test_Matched(string str, string format, params NameValue[] expectedPairs)
        {
            var result = FormattedStringValueExtracter.Extract(str, format);
            result.IsMatch.ShouldBe(true);

            if (expectedPairs == null)
            {
                result.Matches.Count.ShouldBe(0);
                return;
            }

            result.Matches.Count.ShouldBe(expectedPairs.Length);

            for (int i = 0; i < expectedPairs.Length; i++)
            {
                var actualMatch = result.Matches[i];
                var expectedPair = expectedPairs[i];

                actualMatch.Name.ShouldBe(expectedPair.Name);
                actualMatch.Value.ShouldBe(expectedPair.Value);
            }
        }

        private void Test_Not_Matched(string str, string format)
        {
            var result = FormattedStringValueExtracter.Extract(str, format);
            result.IsMatch.ShouldBe(false);
        }
    }
}