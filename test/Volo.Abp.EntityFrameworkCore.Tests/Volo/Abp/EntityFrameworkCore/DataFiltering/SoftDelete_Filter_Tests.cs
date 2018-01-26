using System;
using System.Linq;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestApp.Domain;
using Xunit;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering
{
    public class SoftDelete_Filter_Tests : EntityFrameworkCoreTestBase
    {
        private readonly IRepository<Person, Guid> _personRepository;
        private readonly IDataFilter _dataFilter;

        public SoftDelete_Filter_Tests()
        {
            _personRepository = GetRequiredService<IRepository<Person, Guid>>();
            _dataFilter = GetRequiredService<IDataFilter>();
        }

        [Fact]
        public void Should_Not_Get_Deleted_Entities_By_Default()
        {
            WithUnitOfWork(() =>
            {
                var people = _personRepository.ToList();
                people.Count.ShouldBe(1);
                people.Any(p => p.Name == "Douglas").ShouldBeTrue();
            });
        }

        [Fact]
        public void Should_Get_Deleted_Entities_When_Filter_Is_Disabled()
        {
            WithUnitOfWork(() =>
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
            });
        }
    }
}
