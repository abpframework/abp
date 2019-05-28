using Shouldly;
using Xunit;

namespace System
{
    public class AbpTypeExtensions_Tests
    {
        [Fact]
        public void GetBaseClasses()
        {
            var baseClasses = typeof(MyClass).GetBaseClasses(includeObject: false);
            baseClasses.Length.ShouldBe(2);
            baseClasses[0].ShouldBe(typeof(MyBaseClass1));
            baseClasses[1].ShouldBe(typeof(MyBaseClass2));
        }

        public abstract class MyBaseClass1
        {

        }

        public class MyBaseClass2 : MyBaseClass1
        {

        }

        public class MyClass : MyBaseClass2
        {

        }
    }
}
