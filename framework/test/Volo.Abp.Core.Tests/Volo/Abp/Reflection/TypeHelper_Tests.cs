using System;
using System.Collections.Generic;
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

        [Fact]
        public void IsEnumerable()
        {
            TypeHelper.IsEnumerable(
                typeof(IEnumerable<string>),
                out var itemType
            ).ShouldBeTrue();
            itemType.ShouldBe(typeof(string));

            TypeHelper.IsEnumerable(
                typeof(List<TypeHelper_Tests>),
                out itemType
            ).ShouldBeTrue();
            itemType.ShouldBe(typeof(TypeHelper_Tests));

            TypeHelper.IsEnumerable(
                typeof(TypeHelper_Tests),
                out itemType
            ).ShouldBeFalse();
        }

        [Fact]
        public void IsDictionary()
        {
            //Dictionary<string, int>
            TypeHelper.IsDictionary(
                typeof(Dictionary<string, int>),
                out var keyType,
                out var valueType
            ).ShouldBeTrue();
            keyType.ShouldBe(typeof(string));
            valueType.ShouldBe(typeof(int));

            //MyDictionary
            TypeHelper.IsDictionary(
                typeof(MyDictionary),
                out keyType,
                out valueType
            ).ShouldBeTrue();
            keyType.ShouldBe(typeof(bool));
            valueType.ShouldBe(typeof(TypeHelper_Tests));

            //TypeHelper_Tests
            TypeHelper.IsDictionary(
                typeof(TypeHelper_Tests),
                out keyType,
                out valueType
            ).ShouldBeFalse();
        }

        public class MyDictionary : Dictionary<bool, TypeHelper_Tests>
        {

        }
    }
}
