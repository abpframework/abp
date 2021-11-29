using System.Runtime.Serialization;
using Shouldly;
using Xunit;

namespace Volo.Abp
{
    public class ObjectHelper_Tests
    {
        [Fact]
        public void TrySetProperty_Test()
        {
            var testClass = new MyClass();

            ObjectHelper.TrySetProperty(testClass, x => x.Name, () => "NewName");
            testClass.Name.ShouldBe("NewName");

            ObjectHelper.TrySetProperty(testClass, x => x.Name2, () => "NewName2");
            testClass.Name2.ShouldBe("NewName2");

            ObjectHelper.TrySetProperty(testClass, x => x.Name3, () => "NewName3");
            testClass.Name3.ShouldBe("NewName3");

            ObjectHelper.TrySetProperty(testClass, x => x.Name4, () => "NewName4");
            testClass.Name4.ShouldBe("Name4"); // readonly

            ObjectHelper.TrySetProperty(testClass, x => x.Name5, () => "NewName5", ignoreAttributeTypes: typeof(IgnoreDataMemberAttribute));
            testClass.Name5.ShouldNotBe("NewName5"); // ignore by attribute

            ObjectHelper.TrySetProperty(testClass, x => x.ChildClass.Name, () => "NewChildName");
            testClass.ChildClass.Name.ShouldNotBe("NewChildName");

            ObjectHelper.TrySetProperty(testClass.ChildClass, x => x.Name, () => "NewChildName");
            testClass.ChildClass.Name.ShouldBe("NewChildName");

            ObjectHelper.TrySetProperty(testClass.ChildClass, x => x, () => new MyChildClass
            {
                Name = "NewChildName"
            });
            testClass.ChildClass.Name.ShouldBe("NewChildName");
        }

        class MyClass
        {
            public string Name { get; set; }

            public string Name2 { get; protected set; }

            public string Name3 { get; private set; }

            public string Name4 { get; }

            [IgnoreDataMember]
            public string Name5 { get; }

            public MyChildClass ChildClass { get; set; }

            public MyClass()
            {
                Name = "Name";
                Name2 = "Name2";
                Name3 = "Name3";
                Name4 = "Name4";
                Name5 = "Name5";
                ChildClass = new MyChildClass();
            }
        }

        class MyChildClass
        {
            public string Name { get; set; }

            public MyChildClass()
            {
                Name = "Name";
            }
        }
    }
}
