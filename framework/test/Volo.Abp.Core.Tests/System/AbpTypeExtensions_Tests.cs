using Shouldly;
using Xunit;

namespace System
{
    public class AbpTypeExtensions_Tests
    {
        [Fact]
        public void GetBaseClasses_Excluding_Object()
        {
            var baseClasses = typeof(MyClass).GetBaseClasses(includeObject: false);
            baseClasses.Length.ShouldBe(2);
            baseClasses[0].ShouldBe(typeof(MyBaseClass1));
            baseClasses[1].ShouldBe(typeof(MyBaseClass2));
        }

        [Fact]
        public void GetBaseClasses_With_StoppingType()
        {
            var baseClasses = typeof(MyClass).GetBaseClasses(typeof(MyBaseClass1));
            baseClasses.Length.ShouldBe(1);
            baseClasses[0].ShouldBe(typeof(MyBaseClass2));
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
