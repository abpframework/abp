using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace BaseManagement
{
    [RemoteService]
    [Area("baseManagement")]
    [Route("api/baseManagement/baseType")]
    public class BaseTypeController : AbpController, IBaseTypeAppService
    {
        private readonly IBaseTypeAppService _baseTypeAppService;

        public BaseTypeController(IBaseTypeAppService baseTypeAppService)
        {
            _baseTypeAppService = baseTypeAppService;
        }

        [HttpGet]
        [Route("")]
        public Task<PagedResultDto<BaseTypeDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return _baseTypeAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public Task<BaseTypeDto> GetAsync(Guid id)
        {
            return _baseTypeAppService.GetAsync(id);
        }

        [HttpPost]
        public Task<BaseTypeDto> CreateAsync(CreateUpdateBaseTypeDto input)
        {
            return _baseTypeAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public Task<BaseTypeDto> UpdateAsync(Guid id, CreateUpdateBaseTypeDto input)
        {
            return _baseTypeAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        public Task DeleteAsync(Guid id)
        {
            return _baseTypeAppService.DeleteAsync(id);
        }

     
    }
}
