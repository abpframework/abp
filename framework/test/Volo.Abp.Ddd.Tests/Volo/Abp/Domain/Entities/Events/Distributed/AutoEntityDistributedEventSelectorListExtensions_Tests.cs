using Shouldly;
using Xunit;

namespace Volo.Abp.Domain.Entities.Events.Distributed
{
    public class AutoEntityDistributedEventSelectorListExtensions_Tests
    {
        [Fact]
        public void Add_Entity()
        {
            var selectors = new AutoEntityDistributedEventSelectorList();
            selectors.Add<MyEntity>();
            
            selectors.IsMatch(typeof(MyEntity)).ShouldBeTrue();
        }

        private class MyEntity : Entity<string>
        {
            
        }
    }
}