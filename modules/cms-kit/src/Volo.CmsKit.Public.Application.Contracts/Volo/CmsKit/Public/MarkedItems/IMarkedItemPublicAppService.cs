﻿using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Public.MarkedItems;

public interface IMarkedItemPublicAppService : IApplicationService
{
    Task<MarkedItemWithToggleDto> GetForToggleAsync(string entityType, string entityId);
    Task<bool> ToggleAsync(string entityType, string entityId);
}