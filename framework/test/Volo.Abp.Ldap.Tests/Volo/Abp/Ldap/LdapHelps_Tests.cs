using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace Volo.Abp.Ldap
{
    public class LdapHelps_Tests
    {

        [Fact]
        public void BuildCondition_With_Value()
        {
            // act
            var res = LdapHelps.BuildCondition("objectClass", "testNameA");

            // assert
            res.ShouldBe("(objectClass=testNameA)");
        }

        [Fact]
        public void BuildCondition_With_Null_Value()
        {
            // act
            var res = LdapHelps.BuildCondition("objectClass", null);

            // assert
            res.ShouldBeEmpty();
        }

        [Fact]
        public void BuildCondition_With_Empty_Value()
        {
            // act
            var res = LdapHelps.BuildCondition("objectClass", "");

            // assert
            res.ShouldBeEmpty();
        }

        [Fact]
        public void BuildCondition_With_WhiteSpace_Value()
        {
            // act
            var res = LdapHelps.BuildCondition("objectClass", "   ");

            // assert
            res.ShouldBeEmpty();
        }

        [Fact]
        public void BuildFilter_With_Null_Condition()
        {
            // act
            var res = LdapHelps.BuildFilter(null);

            // assert
            res.ShouldBe("(&(objectClass=*))");
        }

        [Fact]
        public void BuildFilter_With_Empty_Condition()
        {
            // act
            var res = LdapHelps.BuildFilter(new Dictionary<string, string>());

            // assert
            res.ShouldBe("(&(objectClass=*))");
        }

        [Fact]
        public void BuildFilter_With_Condition()
        {
            // act
            var conditions = new Dictionary<string, string>
            {
                {"objectClass", "testClassA"}, {"objectCategory", "testCategoryA"}, {"name", null}
            };
            var res = LdapHelps.BuildFilter(conditions);

            // assert
            res.ShouldBe("(&(objectClass=testClassA)(objectCategory=testCategoryA))");
        }
    }
}