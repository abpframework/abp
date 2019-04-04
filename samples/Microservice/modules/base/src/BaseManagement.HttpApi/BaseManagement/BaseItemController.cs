using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace BaseManagement
{
    [RemoteService]
    [Area("baseManagement")]
    [Route("api/baseManagement/baseItem")]
    public class BaseItemController : AbpController, IBaseItemAppService
    {
        private readonly IBaseItemAppService _baseItemAppService;

        public BaseItemController(IBaseItemAppService baseItemAppService)
        {
            _baseItemAppService = baseItemAppService;
        }

        [HttpGet]
        [Route("")]
        public Task<PagedResultDto<BaseItemDto>> GetListAsync(BaseItemPagedRequestDto input)
        {
            return _baseItemAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public Task<BaseItemDto> GetAsync(Guid id)
        {
            return _baseItemAppService.GetAsync(id);
        }

        [HttpPost]
        public Task<BaseItemDto> CreateAsync(CreateUpdateBaseItemDto input)
        {
            return _baseItemAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public Task<BaseItemDto> UpdateAsync(Guid id, CreateUpdateBaseItemDto input)
        {
            return _baseItemAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        public Task DeleteAsync(Guid id)
        {
            return _baseItemAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("getViewTrees")]
        public List<ViewTree> GetViewTrees(Guid? id)
        {
            return _baseItemAppService.GetViewTrees(id);
        }
    }
}
