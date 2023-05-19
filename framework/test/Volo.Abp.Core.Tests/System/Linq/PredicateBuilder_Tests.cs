using Shouldly;
using Xunit;

namespace System.Linq;

public class PredicateBuilder_Tests
{
    [Fact]
    public void Test1()
    {
        var args = new TestArgs();
        var predicate = PredicateBuilder.New<TestObj>();

        predicate = predicate.And(t => args.Value == t.Value);

        var func = predicate.Compile();

        args.Value = true;
        var r2 = func(new TestObj { Value = true });
        r2.ShouldBeTrue();

        args.Value = false;
        var r1 = func(new TestObj { Value = false });
        r1.ShouldBeTrue();

        args = new TestArgs { Value = true };
        var r3 = func(new TestObj { Value = false });
        r3.ShouldBeFalse();

        args = new TestArgs { Value = false };
        var r4 = func(new TestObj { Value = false });
        r4.ShouldBeTrue();
    }

    [Fact]
    public void Test2()
    {
        var args = new TestArgs();
        var predicate = PredicateBuilder.New<TestObj>();

        predicate = predicate.And(t => !args.Value);

        var func = predicate.Compile();

        args.Value = true;
        var r2 = func(new TestObj { Value = true });
        r2.ShouldBeFalse();

        args.Value = false;
        var r1 = func(new TestObj { Value = false });
        r1.ShouldBeTrue();

        args = new TestArgs { Value = true };
        var r3 = func(new TestObj { Value = false });
        r3.ShouldBeFalse();

        args = new TestArgs { Value = false };
        var r4 = func(new TestObj { Value = false });
        r4.ShouldBeTrue();
    }

    public class TestArgs
    {
        public bool Value { get; set; }
    }

    public class TestObj
    {
        public bool Value { get; set; }
    }
}
