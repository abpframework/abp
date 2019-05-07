using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Users;

namespace OrganizationService
{
    [RemoteService]
    [Area("organization")]
    [Route("api/organization/abpOrganization")]
    public class OrganizationController : AbpController, IOrganizationAppService
    {
        private readonly IOrganizationAppService _abpOrganizationAppService;

        public OrganizationController(IOrganizationAppService abpOrganizationAppService)
        {
            _abpOrganizationAppService = abpOrganizationAppService;
        }

        [HttpGet]
        [Route("")]
        public Task<PagedResultDto<OrganizationDto>> GetListAsync(OrganizationPagedRequestDto input)
        {
            return _abpOrganizationAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("{id}")]
        public Task<OrganizationDto> GetAsync(Guid id)
        {
            return _abpOrganizationAppService.GetAsync(id);
        }

        [HttpPost]
        public Task<OrganizationDto> CreateAsync(CreateUpdateAbpOrganizationDto input)
        {
            return _abpOrganizationAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public Task<OrganizationDto> UpdateAsync(Guid id, CreateUpdateAbpOrganizationDto input)
        {
            return _abpOrganizationAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        public Task DeleteAsync(Guid id)
        {
            return _abpOrganizationAppService.DeleteAsync(id);
        }
        [HttpGet]
        [Route("getViewTrees")]
        public List<ViewTree> GetViewTrees(Guid? id)
        {
            return  _abpOrganizationAppService.GetViewTrees(id);
        }


        [HttpGet]
        [Route("getUserViewTrees")]
        public async Task<List<ViewTree>> GetUserViewTrees(Guid? id)
        {
            return await _abpOrganizationAppService.GetUserViewTrees(id);
        }


        [HttpPost]
        [Route("setOrganizations")]
        public async Task SetOrganizationsAsync(SetUserOrgaizationDto orgaizationDto)
        {
            await _abpOrganizationAppService.SetOrganizationsAsync(orgaizationDto);
        }
    }
}
