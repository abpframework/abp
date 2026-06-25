using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.PermissionManagement.Localization;

namespace Volo.Abp.PermissionManagement.Blazor.MudBlazor.Components;

public partial class ResourcePermissionManagementModal
{
    [Inject] protected IPermissionAppService PermissionAppService { get; set; } = default!;

    [Inject] protected IUiMessageService UiMessageService { get; set; } = default!;

    [Inject] protected ISnackbar Snackbar { get; set; } = default!;

    [Inject] protected IDialogService DialogService { get; set; } = default!;

    protected bool _isVisible;
    protected bool _createDialogVisible;
    protected bool _editDialogVisible;
    protected MudForm? _createFormRef;
    protected MudForm? _editFormRef;

    public bool HasAnyResourcePermission { get; set; }
    public bool HasAnyResourceProviderKeyLookupService { get; set; }
    protected string? ResourceName { get; set; }
    protected string? ResourceKey { get; set; }
    protected string? ResourceDisplayName { get; set; }
    protected int PageSize { get; set; } = 10;

    protected CreateModel CreateEntity { get; set; } = new CreateModel
    {
        Permissions = new List<ResourcePermissionModel>()
    };
    
    protected SearchProviderKeyInfo? _selectedProviderKey;
    protected string? ProviderKey => _selectedProviderKey?.ProviderKey;
    protected string? ProviderDisplayName => _selectedProviderKey?.ProviderDisplayName;

    public GetResourcePermissionDefinitionListResultDto ResourcePermissionDefinitions { get; set; } = new()
    {
        Permissions = new List<ResourcePermissionDefinitionDto>()
    };
    protected string? CurrentLookupService { get; set; }
    protected List<ResourceProviderDto> ResourceProviderKeyLookupServices { get; set; } = new();
    protected List<SearchProviderKeyInfo> ProviderKeys { get; set; } = new();
    protected GetResourcePermissionListResultDto ResourcePermissionList = new()
    {
        Permissions = new List<ResourcePermissionGrantInfoDto>()
    };

    protected EditModel EditEntity { get; set; } = new EditModel
    {
        Permissions = new List<ResourcePermissionModel>()
    };


    public ResourcePermissionManagementModal()
    {
        LocalizationResource = typeof(AbpPermissionManagementResource);
    }

    public virtual async Task OpenAsync(string resourceName, string resourceKey, string resourceDisplayName)
    {
        try
        {
            ResourceName = resourceName;
            ResourceKey = resourceKey;
            ResourceDisplayName = resourceDisplayName;

            ResourcePermissionDefinitions = await PermissionAppService.GetResourceDefinitionsAsync(ResourceName);
            ResourceProviderKeyLookupServices = (await PermissionAppService.GetResourceProviderKeyLookupServicesAsync(ResourceName)).Providers;

            HasAnyResourcePermission = ResourcePermissionDefinitions.Permissions.Any();
            if (HasAnyResourcePermission)
            {
                HasAnyResourceProviderKeyLookupService = ResourceProviderKeyLookupServices.Count > 0;
            }

            await InvokeAsync(StateHasChanged);

            ResourcePermissionList = await PermissionAppService.GetResourceAsync(ResourceName, ResourceKey);

            _isVisible = true;
            await InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual Task CloseModal()
    {
        _isVisible = false;
        return InvokeAsync(StateHasChanged);
    }

    protected virtual async Task OpenCreateDialogAsync()
    {
        CurrentLookupService = ResourceProviderKeyLookupServices.FirstOrDefault()?.Name;

        _selectedProviderKey = null;
        ProviderKeys = new List<SearchProviderKeyInfo>();
        if (_createFormRef != null)
        {
            await _createFormRef.ResetAsync();
        }

        CreateEntity = new CreateModel
        {
            Permissions = ResourcePermissionDefinitions.Permissions.Select(x => new ResourcePermissionModel
            {
                Name = x.Name,
                DisplayName = x.DisplayName,
                IsGranted = false
            }).ToList()
        };

        _createDialogVisible = true;
        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task SelectedProviderKeyChanged(SearchProviderKeyInfo? value)
    {
        _selectedProviderKey = value;

        if (value != null && CurrentLookupService != null)
        {
            var permissionGrants = await PermissionAppService.GetResourceByProviderAsync(ResourceName!, ResourceKey!, CurrentLookupService, value.ProviderKey);
            foreach (var permission in CreateEntity.Permissions)
            {
                permission.IsGranted = permissionGrants.Permissions.Any(p => p.Name == permission.Name && p.Providers.Contains(CurrentLookupService) && p.IsGranted);
            }
        }

        await InvokeAsync(StateHasChanged);
    }

    private async Task<IEnumerable<SearchProviderKeyInfo>> SearchProviderKeyAsync(string value, CancellationToken token)
    {
        if (value.IsNullOrWhiteSpace() || CurrentLookupService == null)
        {
            ProviderKeys = new List<SearchProviderKeyInfo>();
            return ProviderKeys;
        }

        ProviderKeys = (await PermissionAppService.SearchResourceProviderKeyAsync(ResourceName!, CurrentLookupService, value, 1)).Keys;

        return ProviderKeys;
    }

    protected virtual async Task GrantAllCreateAsync(bool value)
    {
        foreach (var permission in CreateEntity.Permissions)
        {
            permission.IsGranted = value;
        }
        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task GrantAllEditAsync(bool value)
    {
        foreach (var permission in EditEntity.Permissions)
        {
            permission.IsGranted = value;
        }
        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task OpenEditDialogAsync(ResourcePermissionGrantInfoDto permission)
    {
        var resourcePermissions = await PermissionAppService.GetResourceByProviderAsync(ResourceName!, ResourceKey!, permission.ProviderName, permission.ProviderKey);
        EditEntity = new EditModel
        {
            ProviderName = permission.ProviderName,
            ProviderKey = permission.ProviderKey,
            Permissions = resourcePermissions.Permissions.Select(x => new ResourcePermissionModel
            {
                Name = x.Name,
                DisplayName = x.DisplayName,
                IsGranted = x.IsGranted
            }).ToList()
        };

        _editDialogVisible = true;
        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task CloseCreateDialogAsync()
    {
        _createDialogVisible = false;
        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task CloseEditDialogAsync()
    {
        _editDialogVisible = false;
        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task OnLookupServiceCheckedValueChanged(string? value)
    {
        CurrentLookupService = value;
        _selectedProviderKey = null;
        ProviderKeys = new List<SearchProviderKeyInfo>();
        if (_createFormRef != null)
        {
            await _createFormRef.ResetAsync();
        }
        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task CreateResourcePermissionAsync()
    {
        if (_createFormRef == null)
        {
            return;
        }

        await _createFormRef.ValidateAsync();
        if (!_createFormRef.IsValid)
        {
            return;
        }

        if (ProviderKey == null || CurrentLookupService == null)
        {
            return;
        }

        await PermissionAppService.UpdateResourceAsync(
            ResourceName!,
            ResourceKey!,
            new UpdateResourcePermissionsDto
            {
                ProviderName = CurrentLookupService,
                ProviderKey = ProviderKey,
                Permissions = CreateEntity.Permissions.Where(p => p.IsGranted).Select(p => p.Name).ToList()
            }
        );

        await CloseCreateDialogAsync();
        ResourcePermissionList = await PermissionAppService.GetResourceAsync(ResourceName!, ResourceKey!);
        await InvokeAsync(StateHasChanged);
        Snackbar.Add(L["SavedSuccessfully"], Severity.Success);
    }

    protected virtual async Task UpdateResourcePermissionAsync()
    {
        if (_editFormRef == null)
        {
            return;
        }

        await _editFormRef.ValidateAsync();
        if (!_editFormRef.IsValid)
        {
            return;
        }
        
        await PermissionAppService.UpdateResourceAsync(
            ResourceName!,
            ResourceKey!,
            new UpdateResourcePermissionsDto
            {
                ProviderName = EditEntity.ProviderName,
                ProviderKey = EditEntity.ProviderKey,
                Permissions = EditEntity.Permissions.Where(p => p.IsGranted).Select(p => p.Name).ToList()
            }
        );

        await CloseEditDialogAsync();
        ResourcePermissionList = await PermissionAppService.GetResourceAsync(ResourceName!, ResourceKey!);
        await InvokeAsync(StateHasChanged);
        Snackbar.Add(L["SavedSuccessfully"], Severity.Success);
    }

    protected virtual async Task DeleteResourcePermissionAsync(ResourcePermissionGrantInfoDto permission)
    {
        var confirmed = await DialogService.ShowMessageBoxAsync(
            L["AreYouSure"],
            L["ResourcePermissionDeletionConfirmationMessage"],
            yesText: L["Yes"],
            cancelText: L["Cancel"]);

        if (confirmed == true)
        {
            await PermissionAppService.DeleteResourceAsync(
                ResourceName!,
                ResourceKey!,
                permission.ProviderName,
                permission.ProviderKey
            );

            ResourcePermissionList = await PermissionAppService.GetResourceAsync(ResourceName!, ResourceKey!);
            Snackbar.Add(L["DeletedSuccessfully"], Severity.Success);
            await InvokeAsync(StateHasChanged);
        }
    }

    public class CreateModel
    {
        public List<ResourcePermissionModel> Permissions { get; set; } = new();
    }

    public class EditModel
    {
        public string ProviderName { get; set; } = string.Empty;

        public string ProviderKey { get; set; } = string.Empty;

        public List<ResourcePermissionModel> Permissions { get; set; } = new();
    }

    public class ResourcePermissionModel
    {
        public string Name { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public bool IsGranted { get; set; }
    }
}
