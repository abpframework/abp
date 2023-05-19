using System;
using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace Volo.Abp;

public class Check_Tests
{
    [Fact]
    public void NotNull_Test()
    {
        Check.NotNull("test", nameof(NotNull_Test)).ShouldBe("test");
        Check.NotNull(string.Empty, nameof(NotNull_Test)).ShouldBe(string.Empty);
        Check.NotNull("test", nameof(NotNull_Test), maxLength: 4, minLength: 0).ShouldBe("test");

        Assert.Throws<ArgumentNullException>(() => Check.NotNull<object>(null, nameof(NotNull_Test)));
        Assert.Throws<ArgumentException>(() => Check.NotNull(null, nameof(NotNull_Test)));
        Assert.Throws<ArgumentException>(() => Check.NotNull("test", nameof(NotNull_Test), maxLength: 3));
        Assert.Throws<ArgumentException>(() => Check.NotNull("test", nameof(NotNull_Test), minLength: 5));
    }

    [Fact]
    public void NotNullOrWhiteSpace_Test()
    {
        Check.NotNullOrWhiteSpace("test", nameof(NotNullOrWhiteSpace_Test)).ShouldBe("test");
        Check.NotNullOrWhiteSpace("test", nameof(NotNullOrWhiteSpace_Test), maxLength: 4, minLength: 0).ShouldBe("test");

        Assert.Throws<ArgumentException>(() => Check.NotNullOrWhiteSpace(null, nameof(NotNullOrWhiteSpace_Test)));
        Assert.Throws<ArgumentException>(() => Check.NotNullOrWhiteSpace(string.Empty, nameof(NotNullOrWhiteSpace_Test)));
        Assert.Throws<ArgumentException>(() => Check.NotNullOrWhiteSpace("test", nameof(NotNullOrWhiteSpace_Test), maxLength: 3));
        Assert.Throws<ArgumentException>(() => Check.NotNullOrWhiteSpace("test", nameof(NotNullOrWhiteSpace_Test), minLength: 5));
    }

    [Fact]
    public void NotNullOrEmpty_Test()
    {
        Check.NotNullOrEmpty("test", nameof(NotNullOrEmpty_Test)).ShouldBe("test");
        Check.NotNullOrEmpty("test", nameof(NotNullOrEmpty_Test), maxLength: 4, minLength: 0).ShouldBe("test");
        Check.NotNullOrEmpty(new List<string>{"test"}, nameof(NotNullOrEmpty_Test));
        
        Assert.Throws<ArgumentException>(() => Check.NotNullOrEmpty(null, nameof(NotNullOrEmpty_Test)));
        Assert.Throws<ArgumentException>(() => Check.NotNullOrEmpty(string.Empty, nameof(NotNullOrEmpty_Test)));
        Assert.Throws<ArgumentException>(() => Check.NotNullOrEmpty("test", nameof(NotNullOrEmpty_Test), maxLength: 3));
        Assert.Throws<ArgumentException>(() => Check.NotNullOrEmpty("test", nameof(NotNullOrEmpty_Test), minLength: 5));
        Assert.Throws<ArgumentException>(() => Check.NotNullOrEmpty(new List<string>(), nameof(NotNullOrEmpty_Test)));
    }

    [Fact]
    public void AssignableTo_Test()
    {
        Check.AssignableTo<object>(typeof(string), nameof(AssignableTo_Test)).ShouldBe(typeof(string));
        Check.AssignableTo<Parent>(typeof(Child), nameof(AssignableTo_Test)).ShouldBe(typeof(Child));
        Check.AssignableTo<Child>(typeof(Child2), nameof(AssignableTo_Test)).ShouldBe(typeof(Child2));
        Check.AssignableTo<Parent>(typeof(Child2), nameof(AssignableTo_Test)).ShouldBe(typeof(Child2));
        
        Assert.Throws<ArgumentException>(() => Check.AssignableTo<Child>(typeof(Parent), nameof(AssignableTo_Test)));
        Assert.Throws<ArgumentException>(() => Check.AssignableTo<Child2>(typeof(Child), nameof(AssignableTo_Test)));
        Assert.Throws<ArgumentException>(() => Check.AssignableTo<Child2>(typeof(Parent), nameof(AssignableTo_Test)));
    }

    [Fact]
    public void Length_Test()
    {
        Check.Length("test", nameof(Length_Test), maxLength: 4).ShouldBe("test");
        Check.Length("test", nameof(Length_Test), maxLength: 5).ShouldBe("test");
        Check.Length("test", nameof(Length_Test), maxLength:4, minLength: 0).ShouldBe("test");
        Check.Length("test", nameof(Length_Test), maxLength:4, minLength: 4).ShouldBe("test");
        
        Assert.Throws<ArgumentException>(() => Check.Length("test", nameof(Length_Test), maxLength: 0));
        Assert.Throws<ArgumentException>(() => Check.Length("test", nameof(Length_Test), maxLength: 3));
        Assert.Throws<ArgumentException>(() => Check.Length("test", nameof(Length_Test), maxLength: 4, minLength: 5));
    }

    [Fact]
    public void Positive_Test()
    {
        Check.Positive(1.To<Int16>(), nameof(Positive_Test)).ShouldBe(1.To<Int16>());
        Check.Positive(1.To<Int32>(), nameof(Positive_Test)).ShouldBe(1.To<Int32>());
        Check.Positive(1.To<Int64>(), nameof(Positive_Test)).ShouldBe(1.To<Int64>());
        Check.Positive(Decimal.One, nameof(Positive_Test)).ShouldBe(Decimal.One);
        Check.Positive(1.0f, nameof(Positive_Test)).ShouldBe(1.0f);
        Check.Positive(1.0, nameof(Positive_Test)).ShouldBe(1.0);
        
        Assert.Throws<ArgumentException>(() => Check.Positive(0.To<Int16>(), nameof(Positive_Test)));
        Assert.Throws<ArgumentException>(() => Check.Positive(0.To<Int32>(), nameof(Positive_Test)));
        Assert.Throws<ArgumentException>(() => Check.Positive(0.To<Int64>(), nameof(Positive_Test)));
        Assert.Throws<ArgumentException>(() => Check.Positive(Decimal.Zero, nameof(Positive_Test)));
        Assert.Throws<ArgumentException>(() => Check.Positive(0.0f, nameof(Positive_Test)));
        Assert.Throws<ArgumentException>(() => Check.Positive(0.0, nameof(Positive_Test)));
        Assert.Throws<ArgumentException>(() => Check.Positive(-1.To<Int16>(), nameof(Positive_Test)));
        Assert.Throws<ArgumentException>(() => Check.Positive(-1.To<Int32>(), nameof(Positive_Test)));
        Assert.Throws<ArgumentException>(() => Check.Positive(-1.To<Int64>(), nameof(Positive_Test)));
        Assert.Throws<ArgumentException>(() => Check.Positive(-Decimal.One, nameof(Positive_Test)));
        Assert.Throws<ArgumentException>(() => Check.Positive(-1.0f, nameof(Positive_Test)));
        Assert.Throws<ArgumentException>(() => Check.Positive(-1.0, nameof(Positive_Test)));
    }

    [Fact]
    public void Range_Test()
    {
        Check.Range(1.To<Int16>(), nameof(Range_Test), minimumValue: 1.To<Int16>(), maximumValue: 10.To<Int16>()).ShouldBe(1.To<Int16>());
        Check.Range(1.To<Int32>(), nameof(Range_Test), minimumValue: 1.To<Int32>(), maximumValue: 10.To<Int32>()).ShouldBe(1.To<Int32>());
        Check.Range(1.To<Int64>(), nameof(Range_Test), minimumValue: 1.To<Int64>(), maximumValue: 10.To<Int64>()).ShouldBe(1.To<Int64>());
        Check.Range(Decimal.One, nameof(Range_Test), minimumValue: Decimal.One, maximumValue: 10.To<Decimal>()).ShouldBe(Decimal.One);
        Check.Range(1.0f, nameof(Range_Test), minimumValue: 1.0f, maximumValue: 10.0f).ShouldBe(1.0f);
        Check.Range(1.0, nameof(Range_Test), minimumValue: 1.0, maximumValue: 10.0).ShouldBe(1.0);
        
        Assert.Throws<ArgumentException>(() => Check.Range(0.To<Int16>(), nameof(Range_Test), minimumValue: 1.To<Int16>(), maximumValue: 10.To<Int16>()));
        Assert.Throws<ArgumentException>(() => Check.Range(0.To<Int32>(), nameof(Range_Test), minimumValue: 1.To<Int32>(), maximumValue: 10.To<Int32>()));
        Assert.Throws<ArgumentException>(() => Check.Range(0.To<Int64>(), nameof(Range_Test), minimumValue: 1.To<Int64>(), maximumValue: 10.To<Int64>()));
        Assert.Throws<ArgumentException>(() => Check.Range(Decimal.Zero, nameof(Range_Test), minimumValue: Decimal.One, maximumValue: 10.To<Decimal>()));
        Assert.Throws<ArgumentException>(() => Check.Range(0.0f, nameof(Range_Test), minimumValue: 1.0f, maximumValue: 10.0f));
        Assert.Throws<ArgumentException>(() => Check.Range(0.0, nameof(Range_Test), minimumValue: 1.0, maximumValue: 10.0));
        Assert.Throws<ArgumentException>(() => Check.Range(11.To<Int16>(), nameof(Range_Test), minimumValue: 1.To<Int16>(), maximumValue: 10.To<Int16>()));
        Assert.Throws<ArgumentException>(() => Check.Range(11.To<Int32>(), nameof(Range_Test), minimumValue: 1.To<Int32>(), maximumValue: 10.To<Int32>()));
        Assert.Throws<ArgumentException>(() => Check.Range(11.To<Int64>(), nameof(Range_Test), minimumValue: 1.To<Int64>(), maximumValue: 10.To<Int64>()));
        Assert.Throws<ArgumentException>(() => Check.Range(11.To<Decimal>(), nameof(Range_Test), minimumValue: Decimal.One, maximumValue: 10.To<Decimal>()));
        Assert.Throws<ArgumentException>(() => Check.Range(11.0f, nameof(Range_Test), minimumValue: 1.0f, maximumValue: 10.0f));
        Assert.Throws<ArgumentException>(() => Check.Range(11.0, nameof(Range_Test), minimumValue: 1.0, maximumValue: 10.0));
    }
    
    class Parent
    {
        
    }
    
    class Child: Parent
    {
        
    }

    class Child2: Child
    {
        
    }
}