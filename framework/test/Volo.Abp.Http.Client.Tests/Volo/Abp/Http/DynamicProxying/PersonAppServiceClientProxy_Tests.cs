using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSubstitute.Extensions;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.Conventions;
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
            people.Items.Count.ShouldBe((int)people.TotalCount);
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
            }, new[] { "name1", "name2" });

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
            });

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
                });
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

            result.FileName.ShouldBe("download.rtf");
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

            var result = await _peopleAppService.UploadAsync(new RemoteStreamContent(memoryStream, "upload.rtf", "application/rtf"));
            result.ShouldBe("UploadAsync:application/rtf:upload.rtf");
        }

        [Fact]
        public async Task UploadPartialAsync()
        {
            var memoryStream = new MemoryStream();
            var rawData = new byte[16];
            var text = Encoding.UTF8.GetBytes("UploadAsync");
            await memoryStream.WriteAsync(rawData);
            await memoryStream.WriteAsync(text);
            memoryStream.Position = rawData.Length;

            var result = await _peopleAppService.UploadAsync(new RemoteStreamContent(memoryStream, "upload.rtf", "application/rtf"));
            result.ShouldBe("UploadAsync:application/rtf:upload.rtf");
        }

        [Fact]
        public async Task UploadMultipleAsync()
        {
            var memoryStream = new MemoryStream();
            await memoryStream.WriteAsync(Encoding.UTF8.GetBytes("File1"));
            memoryStream.Position = 0;

            var memoryStream2 = new MemoryStream();
            await memoryStream2.WriteAsync(Encoding.UTF8.GetBytes("File2"));
            memoryStream2.Position = 0;

            var result = await _peopleAppService.UploadMultipleAsync(new List<IRemoteStreamContent>()
            {
                new RemoteStreamContent(memoryStream, "File1.rtf", "application/rtf"),
                new RemoteStreamContent(memoryStream2, "File2.rtf", "application/rtf2")
            });
            result.ShouldBe("File1:application/rtf:File1.rtfFile2:application/rtf2:File2.rtf");
        }

        [Fact]
        public async Task CreateFileAsync()
        {
            var memoryStream = new MemoryStream();
            await memoryStream.WriteAsync(Encoding.UTF8.GetBytes("CreateFileAsync"));
            memoryStream.Position = 0;
            var result = await _peopleAppService.CreateFileAsync(new CreateFileInput()
            {
                Name = "123.rtf",
                Content = new RemoteStreamContent(memoryStream, "create.rtf", "application/rtf")
            });
            result.ShouldBe("123.rtf:CreateFileAsync:application/rtf:create.rtf");
        }

        [Fact]
        public async Task CreateMultipleFileAsync()
        {
            var memoryStream = new MemoryStream();
            await memoryStream.WriteAsync(Encoding.UTF8.GetBytes("File1"));
            memoryStream.Position = 0;

            var memoryStream2 = new MemoryStream();
            await memoryStream2.WriteAsync(Encoding.UTF8.GetBytes("File2"));
            memoryStream2.Position = 0;

            var memoryStream3 = new MemoryStream();
            await memoryStream3.WriteAsync(Encoding.UTF8.GetBytes("File3"));
            memoryStream3.Position = 0;

            var result = await _peopleAppService.CreateMultipleFileAsync(new CreateMultipleFileInput()
            {
                Name = "123.rtf",
                Contents = new List<RemoteStreamContent>()
                {
                    new RemoteStreamContent(memoryStream, "1-1.rtf", "application/rtf"),
                    new RemoteStreamContent(memoryStream2, "1-2.rtf", "application/rtf2")
                },
                Inner = new CreateFileInput()
                {
                    Name = "789.rtf",
                    Content = new RemoteStreamContent(memoryStream3, "i-789.rtf", "application/rtf3")
                }
            });
            result.ShouldBe("123.rtf:File1:application/rtf:1-1.rtf123.rtf:File2:application/rtf2:1-2.rtf789.rtf:File3:application/rtf3:i-789.rtf");
        }

        [Fact]
        public async Task GetParamsFromQueryAsync()
        {
            var result = await _peopleAppService.GetParamsFromQueryAsync(new GetParamsInput()
            {
                NameValues = new List<GetParamsNameValue>()
                {
                    new GetParamsNameValue()
                    {
                        Name = "name1",
                        Value = "value1"
                    },
                    new GetParamsNameValue()
                    {
                        Name = "name2",
                        Value = "value2"
                    }
                },
                NameValue = new GetParamsNameValue()
                {
                    Name = "name3",
                    Value = "value3"
                }
            });
            result.ShouldBe("name1-value1:name2-value2:name3-value3");
        }

        [Fact]
        public async Task GetParamsFromFormAsync()
        {
            var result = await _peopleAppService.GetParamsFromFormAsync(new GetParamsInput()
            {
                NameValues = new List<GetParamsNameValue>()
                {
                    new GetParamsNameValue()
                    {
                        Name = "name1",
                        Value = "value1"
                    },
                    new GetParamsNameValue()
                    {
                        Name = "name2",
                        Value = "value2"
                    }
                },
                NameValue = new GetParamsNameValue()
                {
                    Name = "name3",
                    Value = "value3"
                }
            });
            result.ShouldBe("name1-value1:name2-value2:name3-value3");
        }
    }
}
