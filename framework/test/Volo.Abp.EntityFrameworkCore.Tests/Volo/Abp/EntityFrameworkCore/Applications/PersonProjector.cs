using System.Linq;
using Volo.Abp.ObjectMapping;
using Volo.Abp.TestApp.Domain;

namespace Volo.Abp.EntityFrameworkCore.Applications;

public class PersonProjector : IQueryableMapper<Person, PersonProjectionDto>
{
    public IQueryable<PersonProjectionDto> ProjectTo(IQueryable<Person> source)
    {
        return source.Select(person => new PersonProjectionDto
        {
            Id = person.Id,
            Name = person.Name
        });
    }
}
