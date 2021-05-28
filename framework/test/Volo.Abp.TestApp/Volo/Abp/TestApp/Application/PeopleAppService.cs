using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.TestApp.Domain;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using Volo.Abp.TestApp.Application.Dto;

namespace Volo.Abp.TestApp.Application
{
    public class PeopleAppService : CrudAppService<Person, PersonDto, Guid>, IPeopleAppService
    {
        public PeopleAppService(IRepository<Person, Guid> repository)
            : base(repository)
        {

        }

        public async Task<ListResultDto<PhoneDto>> GetPhones(Guid id, GetPersonPhonesFilter filter)
        {
            var phones = (await GetEntityByIdAsync(id)).Phones
                .WhereIf(filter.Type.HasValue, p => p.Type == filter.Type)
                .ToList();

            return new ListResultDto<PhoneDto>(
                ObjectMapper.Map<List<Phone>, List<PhoneDto>>(phones)
            );
        }

        public Task<List<string>> GetParams(IEnumerable<Guid> ids, string[] names)
        {
            var @params = ids.Select(id => id.ToString("N")).ToList();
            @params.AddRange(names);
            return Task.FromResult(@params.ToList());
        }

        public async Task<PhoneDto> AddPhone(Guid id, PhoneDto phoneDto)
        {
            var person = await GetEntityByIdAsync(id);
            var phone = new Phone(person.Id, phoneDto.Number, phoneDto.Type);

            person.Phones.Add(phone);
            await Repository.UpdateAsync(person);
            return ObjectMapper.Map<Phone, PhoneDto>(phone);
        }

        public async Task RemovePhone(Guid id, string number)
        {
            var person = await GetEntityByIdAsync(id);
            person.Phones.RemoveAll(p => p.Number == number);
            await Repository.UpdateAsync(person);
        }

        [Authorize]
        public Task GetWithAuthorized()
        {
            return Task.CompletedTask;
        }

        public Task<GetWithComplexTypeInput> GetWithComplexType(GetWithComplexTypeInput input)
        {
            return Task.FromResult(input);
        }

        public async Task<IRemoteStreamContent> DownloadAsync()
        {
            var memoryStream = new MemoryStream();
            await memoryStream.WriteAsync(Encoding.UTF8.GetBytes("DownloadAsync"));
            memoryStream.Position = 0;

            return new RemoteStreamContent(memoryStream, "application/rtf");
        }

        public async Task<string> UploadAsync(IRemoteStreamContent streamContent)
        {
            using (var reader = new StreamReader(streamContent.GetStream()))
            {
                return await reader.ReadToEndAsync() + ":" + streamContent.ContentType;
            }
        }

        public async Task<string> UploadMultipleAsync(IEnumerable<IRemoteStreamContent> streamContents)
        {
            var str = "";
            foreach (var content in streamContents)
            {
                using (var reader = new StreamReader(content.GetStream()))
                {
                    str += await reader.ReadToEndAsync() + ":" + content.ContentType;
                }
            }

            return str;
        }

        public async Task<string> CreateFileAsync(CreateFileInput input)
        {
            using (var reader = new StreamReader(input.Content.GetStream()))
            {
                return input.Name + ":" + await reader.ReadToEndAsync() + ":" + input.Content.ContentType;
            }
        }

        public async Task<string> CreateMultipleFileAsync(CreateMultipleFileInput input)
        {
            var str = "";
            foreach (var content in input.Contents)
            {
                using (var reader = new StreamReader(content.GetStream()))
                {
                    str += input.Name + ":" + await reader.ReadToEndAsync() + ":" + content.ContentType;
                }
            }

            using (var reader = new StreamReader(input.Inner.Content.GetStream()))
            {
                str += input.Inner.Name + ":" + await reader.ReadToEndAsync() + ":" + input.Inner.Content.ContentType;
            }

            return str;
        }
    }
}
