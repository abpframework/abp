using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestApp.Domain;
using Xunit;

namespace Volo.Abp.MemoryDb.DataFilters
{
    public class MemoryDb_SoftDelete_DataFilter_Tests : MemoryDbTestBase
    {
        private readonly IRepository<Person, Guid> _personRepository;
        private readonly IDataFilter _dataFilter;

        public MemoryDb_SoftDelete_DataFilter_Tests()
        {
            _personRepository = ServiceProvider.GetRequiredService<IRepository<Person, Guid>>();
            _dataFilter = GetRequiredService<IDataFilter>();
        }

        [Fact]
        public void Should_Get_Deleted_Entities_When_Filter_Is_Disabled()
        {
            //Soft delete is enabled by default
            var people = _personRepository.ToList();
            people.Any(p => !p.IsDeleted).ShouldBeTrue();
            people.Any(p => p.IsDeleted).ShouldBeFalse();

            using (_dataFilter.Disable<ISoftDelete>())
            {
                //Soft delete is disabled
                people = _personRepository.ToList();
                people.Any(p => !p.IsDeleted).ShouldBeTrue();
                people.Any(p => p.IsDeleted).ShouldBeTrue();

                using (_dataFilter.Enable<ISoftDelete>())
                {
                    //Soft delete is enabled again
                    people = _personRepository.ToList();
                    people.Any(p => !p.IsDeleted).ShouldBeTrue();
                    people.Any(p => p.IsDeleted).ShouldBeFalse();
                }

                //Soft delete is disabled (restored previous state)
                people = _personRepository.ToList();
                people.Any(p => !p.IsDeleted).ShouldBeTrue();
                people.Any(p => p.IsDeleted).ShouldBeTrue();
            }

            //Soft delete is enabled (restored previous state)
            people = _personRepository.ToList();
            people.Any(p => !p.IsDeleted).ShouldBeTrue();
            people.Any(p => p.IsDeleted).ShouldBeFalse();
        }
    }
}
