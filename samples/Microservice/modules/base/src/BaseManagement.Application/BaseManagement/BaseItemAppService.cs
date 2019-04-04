using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace BaseManagement
{
    [Authorize(BaseManagementPermissions.BaseItems.Default)]
    public class BaseItemAppService : AsyncCrudAppService<BaseItem, BaseItemDto, Guid, BaseItemPagedRequestDto,
        CreateUpdateBaseItemDto, CreateUpdateBaseItemDto>, IBaseItemAppService
    {
        private readonly IRepository<BaseItem, Guid> _repository;
        private readonly IRepository<BaseType, Guid> _baseTypeRepository;

        public BaseItemAppService(IRepository<BaseItem, Guid> repository, IRepository<BaseType, Guid> baseTypeRepository)
            : base(repository)
        {
            _repository = repository;
            _baseTypeRepository = baseTypeRepository;
            base.CreatePolicyName = BaseManagementPermissions.BaseItems.Create;
            base.UpdatePolicyName = BaseManagementPermissions.BaseItems.Update;
            base.DeletePolicyName = BaseManagementPermissions.BaseItems.Delete;
        }

        protected override IQueryable<BaseItem> CreateFilteredQuery(BaseItemPagedRequestDto input)
        {
            return base.CreateFilteredQuery(input).WhereIf(input.BaseTypeGuid.HasValue, r => r.BaseTypeGuid == input.BaseTypeGuid);
        }

        public List<ViewTree> GetViewTrees(Guid? id)
        {
            List<ViewTree> viewTrees = _baseTypeRepository.GetList().Select(r => new ViewTree()
            {
                Guid = r.Id,
                Title = r.Name,
                Selected = id == r.Id ? true : false,
                ParentGuid = r.ParentId
            }).ToList();

            return viewTrees.ComboboxTreeJson();
        }
    }
}