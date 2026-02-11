using System.Runtime.Serialization;
using Shouldly;
using Xunit;

namespace Volo.Abp;

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

    [Fact]
    public void TrySetProperty_WithNullableNewValueType_SetsCorrectly()
    {
        // Arrange
        var sut = new AbstractParentImpl();
        long? newValue = 10;


        // Act & Assert
        var sutAsIFirst = (IFirst)sut;

        ObjectHelper.TrySetProperty(sutAsIFirst, x => x.ValueProp1FromIFirst, () => newValue);
        sutAsIFirst.ValueProp1FromIFirst.ShouldBe(newValue.Value);

        ObjectHelper.TrySetProperty(sutAsIFirst, x => x.ValueProp2FromIFirst, () => newValue);
        sutAsIFirst.ValueProp2FromIFirst.ShouldBe(newValue.Value);

        ObjectHelper.TrySetProperty(sutAsIFirst, x => x.ValueProp3FromIFirst, () => newValue);
        sutAsIFirst.ValueProp3FromIFirst.ShouldNotBe(newValue.Value); // private set on implementation not accessible

        ObjectHelper.TrySetProperty(sutAsIFirst, x => x.ValueProp4FromIFirst, () => newValue);
        sutAsIFirst.ValueProp4FromIFirst.ShouldNotBe(newValue.Value); // readonly

        ObjectHelper.TrySetProperty(sutAsIFirst, x => x.ValueProp5FromIFirst, () => newValue,
            ignoreAttributeTypes: typeof(IgnoreDataMemberAttribute));
        sutAsIFirst.ValueProp5FromIFirst.ShouldNotBe(newValue.Value); // ignore by attribute

        var sutAsISecond = (ISecond)sut;
        ObjectHelper.TrySetProperty(sutAsISecond, x => x.ValueProp1FromISecond, () => newValue);
        sutAsISecond.ValueProp1FromISecond.ShouldNotBe(newValue.Value); // readonly
    }

    internal interface IFirst
    {
        public long ValueProp1FromIFirst { get; }

        public long ValueProp2FromIFirst { get; }

        public long ValueProp3FromIFirst { get; }

        public long ValueProp4FromIFirst { get; }

        public long ValueProp5FromIFirst { get; }
    }

    internal interface ISecond
    {
        public long ValueProp1FromISecond { get; }
    }

    internal interface IHasKey<out TKey>
    {
        TKey Id { get; }
    }

    internal interface IHaveMixedProps : IFirst, ISecond
    {
    }

    abstract internal class GenericBase<TKey> : IHasKey<TKey>
    {
        public virtual TKey Id { get; protected set; }
    }

    abstract internal class AbstractParent<TKey> : GenericBase<TKey>, IHaveMixedProps
    {
        public long ValueProp1FromIFirst { get; set; }

        public long ValueProp2FromIFirst { get; protected set; }

        public long ValueProp3FromIFirst { get; private set; }

        public long ValueProp4FromIFirst { get; }

        [IgnoreDataMember] public long ValueProp5FromIFirst { get; }

        public long ValueProp1FromISecond { get; }
    }

    internal class AbstractParentImpl : AbstractParent<long>
    {
        public long OwnProp1 { get; set; }
        public string OwnProp2 { get; set; }
    }

    class MyClass
    {
        public string Name { get; set; }

        public string Name2 { get; protected set; }

        public string Name3 { get; private set; }

        public string Name4 { get; }

        [IgnoreDataMember]
        public string Name5 { get; }

        public long Number { get; set; }

        public long Number2 { get; protected set; }

        public long Number3 { get; private set; }

        public long Number4 { get; }

        [IgnoreDataMember]
        public long Number5 { get; }

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
