using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.TestApp.Domain;

public interface IPersonRepository : IBasicRepository<Person, Guid>
{
    Task<PersonView> GetViewAsync(string name);
}
