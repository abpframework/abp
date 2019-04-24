using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace OrganizationService
{
    [Authorize(OrganizationServicePermissions.AbpOrganizations.Default)]
    public class OrganizationAppService : AsyncCrudAppService<AbpOrganization, AbpOrganizationDto, Guid, AbpOrganizationPagedRequestDto,
        CreateUpdateAbpOrganizationDto, CreateUpdateAbpOrganizationDto>, IOrganizationAppService
    {
        private readonly IRepository<AbpOrganization, Guid> _repository;

        public OrganizationAppService(IRepository<AbpOrganization, Guid> repository)
            : base(repository)
        {
            _repository = repository;
            base.CreatePolicyName = OrganizationServicePermissions.AbpOrganizations.Create;
            base.UpdatePolicyName = OrganizationServicePermissions.AbpOrganizations.Update;
            base.DeletePolicyName = OrganizationServicePermissions.AbpOrganizations.Delete;
        }

        protected override IQueryable<AbpOrganization> CreateFilteredQuery(AbpOrganizationPagedRequestDto input)
        {
            return base.CreateFilteredQuery(input).Where(r => r.ParentId == input.ParentId);
        }


        public override async Task<PagedResultDto<AbpOrganizationDto>> GetListAsync(AbpOrganizationPagedRequestDto input)
        {
            PagedResultDto<AbpOrganizationDto> baseResults = await base.GetListAsync(input);

            foreach (var baseResultsItem in baseResults.Items)
            {
                baseResultsItem.HasChildren = _repository.Any(r => r.ParentId == baseResultsItem.Id);
            }

            return baseResults;
        }

        public List<ViewTree> GetViewTrees(Guid? guid)
        {
            List<ViewTree> viewTrees = _repository.GetList().Select(r => new ViewTree()
            {
                Guid = r.Id,
                Title = r.Name,
                //Selected = guid == r.ParentId ? true : false,
                ParentGuid = r.ParentId
            }).ToList();


            var viewTree = viewTrees.FirstOrDefault(r => r.Guid == guid);
            if (viewTree != null)
            {
                viewTrees.ForEach(r => { r.Selected = r.Guid == viewTree.ParentGuid ? true : false; });
            }

            return viewTrees.ComboboxTreeJson();
        }
    }
}