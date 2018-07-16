using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using Xunit;

namespace Volo.Abp.Reflection
{
    public class TypeHelper_Tests
    {
        [Fact]
        public void Should_Generic_Type_From_Nullable()
        {
            var nullableType = typeof(Guid?);
            var guidType = nullableType.GetFirstGenericArgumentIfNullable();

            guidType.ShouldBe(typeof(Guid));
        }
    }
}
