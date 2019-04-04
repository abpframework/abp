using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace BaseManagement
{
    [Authorize(BaseManagementPermissions.BaseTypes.Default)]
    public class BaseTypeAppService : AsyncCrudAppService<BaseType, BaseTypeDto, Guid, BaseTypePagedRequestDto,
        CreateUpdateBaseTypeDto, CreateUpdateBaseTypeDto>, IBaseTypeAppService
    {
        private readonly IRepository<BaseType, Guid> _repository;

        public BaseTypeAppService(IRepository<BaseType, Guid> repository)
            : base(repository)
        {
            _repository = repository;
            base.CreatePolicyName = BaseManagementPermissions.BaseTypes.Create;
            base.UpdatePolicyName = BaseManagementPermissions.BaseTypes.Update;
            base.DeletePolicyName = BaseManagementPermissions.BaseTypes.Delete;
        }

        protected override IQueryable<BaseType> CreateFilteredQuery(BaseTypePagedRequestDto input)
        {
            return base.CreateFilteredQuery(input).Where(r => r.ParentId == input.ParentId);
        }


        public override async Task<PagedResultDto<BaseTypeDto>> GetListAsync(BaseTypePagedRequestDto input)
        {
            PagedResultDto<BaseTypeDto> baseResults = await base.GetListAsync(input);

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