using System.Linq;
using Shouldly;
using Xunit;

namespace System.Collections.Generic
{
    public class AbpListExtensions_Tests
    {
        [Fact]
        public void InsertAfter()
        {
            var list = Enumerable.Range(1, 3).ToList();

            list.InsertAfter(2, 42);

            list.Count.ShouldBe(4);
            list[0].ShouldBe(1);
            list[1].ShouldBe(2);
            list[2].ShouldBe(42);
            list[3].ShouldBe(3);

            list.InsertAfter(3, 43);

            list.Count.ShouldBe(5);
            list[0].ShouldBe(1);
            list[1].ShouldBe(2);
            list[2].ShouldBe(42);
            list[3].ShouldBe(3);
            list[4].ShouldBe(43);
        }

        [Fact]
        public void InsertAfter_With_Predicate()
        {
            var list = Enumerable.Range(1, 3).ToList();

            list.InsertAfter(i => i == 2, 42);

            list.Count.ShouldBe(4);
            list[0].ShouldBe(1);
            list[1].ShouldBe(2);
            list[2].ShouldBe(42);
            list[3].ShouldBe(3);

            list.InsertAfter(i => i == 3, 43);

            list.Count.ShouldBe(5);
            list[0].ShouldBe(1);
            list[1].ShouldBe(2);
            list[2].ShouldBe(42);
            list[3].ShouldBe(3);
            list[4].ShouldBe(43);
        }

        [Fact]
        public void InsertAfter_With_Predicate_Should_Insert_To_First_If_Not_Found()
        {
            var list = Enumerable.Range(1, 3).ToList();

            list.InsertAfter(i => i == 999, 42);

            list.Count.ShouldBe(4);
            list[0].ShouldBe(42);
            list[1].ShouldBe(1);
            list[2].ShouldBe(2);
            list[3].ShouldBe(3);
        }

        [Fact]
        public void InsertBefore()
        {
            var list = Enumerable.Range(1, 3).ToList();

            list.InsertBefore(2, 42);

            list.Count.ShouldBe(4);
            list[0].ShouldBe(1);
            list[1].ShouldBe(42);
            list[2].ShouldBe(2);
            list[3].ShouldBe(3);

            list.InsertBefore(1, 43);

            list.Count.ShouldBe(5);
            list[0].ShouldBe(43);
            list[1].ShouldBe(1);
            list[2].ShouldBe(42);
            list[3].ShouldBe(2);
            list[4].ShouldBe(3);
        }

        [Fact]
        public void InsertBefore_With_Predicate()
        {
            var list = Enumerable.Range(1, 3).ToList();

            list.InsertBefore(i => i == 2, 42);

            list.Count.ShouldBe(4);
            list[0].ShouldBe(1);
            list[1].ShouldBe(42);
            list[2].ShouldBe(2);
            list[3].ShouldBe(3);

            list.InsertBefore(i => i == 1, 43);

            list.Count.ShouldBe(5);
            list[0].ShouldBe(43);
            list[1].ShouldBe(1);
            list[2].ShouldBe(42);
            list[3].ShouldBe(2);
            list[4].ShouldBe(3);
        }

        [Fact]
        public void ReplaceWhile_WithValue()
        {
            var list = Enumerable.Range(1, 3).ToList();

            list.ReplaceWhile(i => i >= 2, 42);

            list[0].ShouldBe(1);
            list[1].ShouldBe(42);
            list[2].ShouldBe(42);
        }

        [Fact]
        public void ReplaceWhile_WithFactory()
        {
            var list = Enumerable.Range(1, 3).ToList();

            list.ReplaceWhile(i => i >= 2, i => i + 1);

            list[0].ShouldBe(1);
            list[1].ShouldBe(3);
            list[2].ShouldBe(4);
        }

        [Fact]
        public void ReplaceOne_WithValue()
        {
            var list = Enumerable.Range(1, 3).ToList();

            list.ReplaceOne(i => i >= 2, 42);

            list[0].ShouldBe(1);
            list[1].ShouldBe(42);
            list[2].ShouldBe(3);
        }

        [Fact]
        public void ReplaceOne_WithFactory()
        {
            var list = Enumerable.Range(1, 3).ToList();

            list.ReplaceOne(i => i >= 2, i => i + 1);

            list[0].ShouldBe(1);
            list[1].ShouldBe(3);
            list[2].ShouldBe(3);
        }

        [Fact]
        public void ReplaceOne_With_Item()
        {
            var list = Enumerable.Range(1, 3).ToList();

            list.ReplaceOne(2, 42);

            list[0].ShouldBe(1);
            list[1].ShouldBe(42);
            list[2].ShouldBe(3);
        }
    }
}
