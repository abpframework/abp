﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute.Extensions;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Http.Client;
using Volo.Abp.TestApp.Application;
using Volo.Abp.TestApp.Application.Dto;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.Validation;
using Xunit;

namespace Volo.Abp.Http.DynamicProxying
{
    public class PersonAppServiceClientProxy_Tests : AbpHttpClientTestBase
    {
        private readonly IPeopleAppService _peopleAppService;
        private readonly IRepository<Person, Guid> _personRepository;

        public PersonAppServiceClientProxy_Tests()
        {
            _peopleAppService = ServiceProvider.GetRequiredService<IPeopleAppService>();
            _personRepository = ServiceProvider.GetRequiredService<IRepository<Person, Guid>>();
        }

        [Fact]
        public async Task Get()
        {
            var firstPerson = (await _personRepository.GetListAsync()).First();

            var person = await _peopleAppService.GetAsync(firstPerson.Id);
            person.ShouldNotBeNull();
            person.Id.ShouldBe(firstPerson.Id);
            person.Name.ShouldBe(firstPerson.Name);
        }

        [Fact]
        public async Task GetList()
        {
            var people = await _peopleAppService.GetListAsync(new PagedAndSortedResultRequestDto());
            people.TotalCount.ShouldBeGreaterThan(0);
            people.Items.Count.ShouldBe((int) people.TotalCount);
        }

        [Fact]
        public async Task GetParams()
        {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();

            var @params = await _peopleAppService.GetParams(new List<Guid>
            {
                id1,
                id2
            }, new[] {"name1", "name2"});

            @params.ShouldContain(id1.ToString("N"));
            @params.ShouldContain(id2.ToString("N"));
            @params.ShouldContain("name1");
            @params.ShouldContain("name2");
        }

        [Fact]
        public async Task Delete()
        {
            var firstPerson = (await _personRepository.GetListAsync()).First();

            await _peopleAppService.DeleteAsync(firstPerson.Id);

            firstPerson = (await _personRepository.GetListAsync()).FirstOrDefault(p => p.Id == firstPerson.Id);
            firstPerson.ShouldBeNull();
        }

        [Fact]
        public async Task Create()
        {
            var uniquePersonName = Guid.NewGuid().ToString();

            var person = await _peopleAppService.CreateAsync(new PersonDto
                {
                    Name = uniquePersonName,
                    Age = 42
                }
            );

            person.ShouldNotBeNull();
            person.Id.ShouldNotBe(Guid.Empty);
            person.Name.ShouldBe(uniquePersonName);

            var personInDb = (await _personRepository.GetListAsync()).FirstOrDefault(p => p.Name == uniquePersonName);
            personInDb.ShouldNotBeNull();
            personInDb.Id.ShouldBe(person.Id);
        }

        [Fact]
        public async Task Create_Validate_Exception()
        {
            await Assert.ThrowsAsync<AbpValidationException>(async () =>
            {
                var person = await _peopleAppService.CreateAsync(new PersonDto
                    {
                        Age = 42
                    }
                );
            });
        }

        [Fact]
        public async Task Update()
        {
            var firstPerson = (await _personRepository.GetListAsync()).First();
            var uniquePersonName = Guid.NewGuid().ToString();

            var person = await _peopleAppService.UpdateAsync(
                firstPerson.Id,
                new PersonDto
                {
                    Id = firstPerson.Id,
                    Name = uniquePersonName,
                    Age = firstPerson.Age
                }
            );

            person.ShouldNotBeNull();
            person.Id.ShouldBe(firstPerson.Id);
            person.Name.ShouldBe(uniquePersonName);
            person.Age.ShouldBe(firstPerson.Age);

            var personInDb = (await _personRepository.GetListAsync()).FirstOrDefault(p => p.Id == firstPerson.Id);
            personInDb.ShouldNotBeNull();
            personInDb.Id.ShouldBe(person.Id);
            personInDb.Name.ShouldBe(person.Name);
            personInDb.Age.ShouldBe(person.Age);
        }

        [Fact]
        public async Task GetWithAuthorized()
        {
            await Assert.ThrowsAnyAsync<Exception>(async () =>
            {
                await _peopleAppService.GetWithAuthorized();
            });
        }

        [Fact]
        public async Task GetWithComplexType()
        {
            var result = await _peopleAppService.GetWithComplexType(
                new GetWithComplexTypeInput
                {
                    Value1 = "value one",
                    Inner1 = new GetWithComplexTypeInput.GetWithComplexTypeInner
                    {
                        Value2 = "value two",
                        Inner2 = new GetWithComplexTypeInput.GetWithComplexTypeInnerInner
                        {
                            Value3 = "value three"
                        }
                    }
                }
            );

            result.Value1.ShouldBe("value one");
            result.Inner1.Value2.ShouldBe("value two");
            result.Inner1.Inner2.Value3.ShouldBe("value three");
        }

        [Fact]
        public async Task DownloadAsync()
        {
            var result = await _peopleAppService.DownloadAsync();

            result.ContentType.ShouldBe("application/rtf");
            using (var reader = new StreamReader(result.GetStream()))
            {
                var str = await reader.ReadToEndAsync();
                str.ShouldBe("DownloadAsync");
            }
        }

        [Fact]
        public async Task UploadAsync()
        {
            var memoryStream = new MemoryStream();
            await memoryStream.WriteAsync(Encoding.UTF8.GetBytes("UploadAsync"));
            memoryStream.Position = 0;
            var result = await _peopleAppService.UploadAsync(new RemoteStreamContent(memoryStream)
            {
                ContentType = "application/rtf"
            });
            result.ShouldBe("UploadAsync:application/rtf");
        }
    }
}
