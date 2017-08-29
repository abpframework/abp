using System.Collections.Generic;
using Volo.Abp.Application.Services;

namespace App
{
    public class PersonAppService : ApplicationService
    {
        public List<Person> GetAll()
        {
            return new List<Person>
            {
                new Person("John", 33),
                new Person("Dougles", 42),
            };
        }
    }
}
