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
            //Ideally should be [GET] /api/app/person OK!
            var result = await GetResponseAsObjectAsync<ListResultDto<PersonDto>>("/api/app/person");
            result.Items.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Get_Test()
        {
            var firstPerson = _personRepository.GetList().First();

            //Ideally should be [GET] /api/app/person/{id} OK!
            var result = await GetResponseAsObjectAsync<PersonDto>($"/api/app/person/{firstPerson.Id}");
            result.Name.ShouldBe(firstPerson.Name);
        }

        [Fact]
        public async Task Delete_Test()
        {
            var firstPerson = _personRepository.GetList().First();

            //Ideally should be [DELETE] /api/app/person/{id} OK!
            await Client.DeleteAsync($"/api/app/person/{firstPerson.Id}");

            (await _personRepository.FindAsync(firstPerson.Id)).ShouldBeNull();
        }

        [Fact]
        public async Task Create_Test()
        {
            //Act

            var postData = _jsonSerializer.Serialize(new PersonDto {Name = "John", Age = 33});

            //Ideally should be [POST] /api/app/person OK!
            var response = await Client.PostAsync(
                "/api/app/person",
                new StringContent(postData, Encoding.UTF8, "application/json")
            );

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

            //Ideally should be [PUT] /api/app/person/{id} OK!
            var response = await Client.PutAsync(
                $"/api/app/person/{updateDto.Id}",
                new StringContent(putData, Encoding.UTF8, "application/json")
            );

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

        [Fact]
        public async Task AddPhone_Test()
        {
            //Arrange

            var personToAddNewPhone = _personRepository.GetList().First();
            var phoneNumberToAdd = RandomHelper.GetRandom(1000000, 9000000).ToString();

            //Act

            var postData = _jsonSerializer.Serialize(new PhoneDto { Type = PhoneType.Mobile, Number = phoneNumberToAdd });

            //Ideally should be [POST] /api/people/{id}/phones
            var response = await Client.PostAsync(
                $"/api/app/person/{personToAddNewPhone.Id}/Phone",
                new StringContent(postData, Encoding.UTF8, "application/json")
            );

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var resultAsString = await response.Content.ReadAsStringAsync();
            var resultDto = _jsonSerializer.Deserialize<PhoneDto>(resultAsString);

            //Assert

            resultDto.Type.ShouldBe(PhoneType.Mobile);
            resultDto.Number.ShouldBe(phoneNumberToAdd);

            var personInDb = await _personRepository.FindAsync(personToAddNewPhone.Id);
            personInDb.ShouldNotBeNull();
            personInDb.Phones.Any(p => p.Number == phoneNumberToAdd).ShouldBeTrue();
        }

        [Fact]
        public async Task GetPhones_Test()
        {
            var douglas = _personRepository.GetList().First(p => p.Name == "Douglas");

            //Ideally should be [GET] /api/person/{id}/phones?type=office
            var result = await GetResponseAsObjectAsync<ListResultDto<PhoneDto>>($"/api/app/person/{douglas.Id}/Phones");
            result.Items.Count.ShouldBe(douglas.Phones.Count);
        }

        [Fact]
        public async Task DeletePhone_Test()
        {
            var douglas = _personRepository.GetList().First(p => p.Name == "Douglas");
            var firstPhone = douglas.Phones.First();

            //Ideally should be [DELETE] /api/app/person/{id}
            await Client.DeleteAsync($"/api/app/person/{douglas.Id}/Phone?phoneId=" + firstPhone.Id);

            douglas = _personRepository.GetList().First(p => p.Name == "Douglas");
            douglas.Phones.Any(p => p.Id == firstPhone.Id).ShouldBeFalse();
        }
    }
}
