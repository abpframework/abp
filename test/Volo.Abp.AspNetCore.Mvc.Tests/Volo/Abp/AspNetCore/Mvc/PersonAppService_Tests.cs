using System;
using Shouldly;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application.Dtos;
using Volo.Abp.TestApp.Application;
using Volo.Abp.TestApp.Domain;
using Xunit;
using Volo.Abp.Domain.Repositories;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Json;

namespace Volo.Abp.AspNetCore.Mvc
{
    //TODO: Refactor to make tests easier.

    public class PersonAppService_Tests : AspNetCoreMvcTestBase
    {
        private readonly IQueryableRepository<Person> _personRepository;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IObjectMapper _objectMapper;

        public PersonAppService_Tests()
        {
            _personRepository = ServiceProvider.GetRequiredService<IQueryableRepository<Person>>();
            _jsonSerializer = ServiceProvider.GetRequiredService<IJsonSerializer>();
            _objectMapper = ServiceProvider.GetRequiredService<IObjectMapper>();
        }

        [Fact]
        public async Task GetAll_Test()
        {
            //Ideally should be [GET] /api/app/person
            var result = await GetResponseAsObjectAsync<ListResultDto<PersonDto>>("/api/services/app/person/GetAll");
            result.Items.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Get_Test()
        {
            var firstPerson = _personRepository.GetList().First();

            //Ideally should be [GET] /api/app/person/{id}
            var result = await GetResponseAsObjectAsync<PersonDto>("/api/services/app/person/Get?id=" + firstPerson.Id);
            result.Name.ShouldBe(firstPerson.Name);
        }

        [Fact]
        public async Task Delete_Test()
        {
            var firstPerson = _personRepository.GetList().First();

            //Ideally should be [DELETE] /api/app/person/{id}
            await Client.DeleteAsync("/api/services/app/person/Delete?id=" + firstPerson.Id);

            (await _personRepository.FindAsync(firstPerson.Id)).ShouldBeNull();
        }

        [Fact]
        public async Task Create_Test()
        {
            //Act

            var postData = _jsonSerializer.Serialize(new PersonDto {Name = "John", Age = 33});

            //Ideally should be [POST] /api/app/person
            var response = await Client.PostAsync(
                "/api/services/app/person/Create",
                new StringContent(postData, Encoding.UTF8, "application/json")
            );

            response.IsSuccessStatusCode.ShouldBeTrue();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var resultAsString = await response.Content.ReadAsStringAsync();
            PersonDto resultDto = _jsonSerializer.Deserialize<PersonDto>(resultAsString);

            //Assert

            resultDto.Id.ShouldNotBe(default(Guid));
            resultDto.Name.ShouldBe("John");
            resultDto.Age.ShouldBe(33);

            (await _personRepository.FindAsync(resultDto.Id)).ShouldNotBeNull();
        }


        [Fact]
        public async Task Update_Test()
        {
            //Arrange

            var firstPerson = _personRepository.GetList().First();
            var firstPersonAge = firstPerson.Age; //Persist to a variable since we are using in-memory database which shares same entity.
            var updateDto = _objectMapper.Map<Person, PersonDto>(firstPerson);
            updateDto.Age = updateDto.Age + 1;
            var putData = _jsonSerializer.Serialize(updateDto);

            //Act

            //Ideally should be [PUT] /api/app/person
            var response = await Client.PutAsync(
                "/api/services/app/person/Update/{id}",
                new StringContent(putData, Encoding.UTF8, "application/json")
            );

            response.IsSuccessStatusCode.ShouldBeTrue();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var resultAsString = await response.Content.ReadAsStringAsync();
            PersonDto resultDto = _jsonSerializer.Deserialize<PersonDto>(resultAsString);

            //Assert

            resultDto.Id.ShouldBe(firstPerson.Id);
            resultDto.Name.ShouldBe(firstPerson.Name);
            resultDto.Age.ShouldBe(firstPersonAge + 1);

            var personInDb = (await _personRepository.FindAsync(resultDto.Id));
            personInDb.ShouldNotBeNull();
            personInDb.Name.ShouldBe(firstPerson.Name);
            personInDb.Age.ShouldBe(firstPersonAge + 1);
        }
    }
}
