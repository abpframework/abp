using System.Collections;
using System.Collections.Generic;
using Shouldly;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Reflection;
using Xunit;

namespace Volo.Abp.Http;

public class ApiTypeNameHelper_Tests
{
    [Fact]
    public void GetTypeName_Test()
    {
        ApiTypeNameHelper.GetTypeName(typeof(CycleClass)).ShouldBe(TypeHelper.GetFullNameHandlingNullableAndGenerics(typeof(CycleClass)));
        ApiTypeNameHelper.GetTypeName(typeof(CycleClass2)).ShouldBe(TypeHelper.GetFullNameHandlingNullableAndGenerics(typeof(CycleClass2)));
        ApiTypeNameHelper.GetTypeName(typeof(CycleClass3)).ShouldBe($"[{TypeHelper.GetFullNameHandlingNullableAndGenerics(typeof(CycleClass4))}]");
    }

    [Fact]
    public void GetSimpleTypeName_Test()
    {
        ApiTypeNameHelper.GetSimpleTypeName(typeof(CycleClass)).ShouldBe(TypeHelper.GetSimplifiedName(typeof(CycleClass)));
        ApiTypeNameHelper.GetSimpleTypeName(typeof(CycleClass2)).ShouldBe(TypeHelper.GetSimplifiedName(typeof(CycleClass2)));
        ApiTypeNameHelper.GetTypeName(typeof(CycleClass3)).ShouldBe($"[{TypeHelper.GetSimplifiedName(typeof(CycleClass4))}]");
    }

    class CycleClass : IEnumerable<CycleClass>
    {
        public IEnumerator<CycleClass> GetEnumerator()
        {
            yield return new CycleClass();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    class CycleClass2 : Dictionary<CycleClass2, CycleClass2>
    {

    }

    class CycleClass3 : IEnumerable<CycleClass4>
    {

        public IEnumerator<CycleClass4> GetEnumerator()
        {
            yield return new CycleClass4();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    class CycleClass4 : IEnumerable<CycleClass3>
    {
        public IEnumerator<CycleClass3> GetEnumerator()
        {
            yield return new CycleClass3();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
