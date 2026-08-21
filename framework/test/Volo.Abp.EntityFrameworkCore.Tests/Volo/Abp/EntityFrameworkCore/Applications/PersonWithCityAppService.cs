#nullable enable
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.EntityFrameworkCore.Applications;

//City is another aggregate root, so Person has no City navigation property to project.
public class PersonWithCityAppService : ReadOnlyAppService<Person, PersonWithCityDto, Guid>
{
    private readonly IReadOnlyRepository<City, Guid> _cityRepository;

    public PersonWithCityAppService(
        IReadOnlyRepository<Person, Guid> repository,
        IReadOnlyRepository<City, Guid> cityRepository)
        : base(repository)
    {
        _cityRepository = cityRepository;
    }

    protected override async Task<IQueryable<PersonWithCityDto>?> CreateGetOutputDtoQueryOrNullAsync(Guid id)
    {
        var people = await CreateEntityQueryOrNullAsync(id);

        return people == null ? null : await JoinCitiesAsync(people);
    }

    protected override async Task<IQueryable<PersonWithCityDto>?> CreateGetListOutputDtoQueryOrNullAsync(IQueryable<Person> query)
    {
        return await JoinCitiesAsync(query);
    }

    //left join, an inner join would drop the people without a city and break the total count
    private async Task<IQueryable<PersonWithCityDto>> JoinCitiesAsync(IQueryable<Person> people)
    {
        var cities = await _cityRepository.GetQueryableAsync();

        return from person in people
               join city in cities on person.CityId equals city.Id into personCities
               from personCity in personCities.DefaultIfEmpty()
               select new PersonWithCityDto
               {
                   Id = person.Id,
                   Name = person.Name,
                   CityName = personCity != null ? personCity.Name : null
               };
    }
}
