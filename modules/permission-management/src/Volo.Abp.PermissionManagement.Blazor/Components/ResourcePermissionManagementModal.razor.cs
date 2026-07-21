using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.Components;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.PermissionManagement.Localization;

namespace Volo.Abp.PermissionManagement.Blazor.Components;

public partial class ResourcePermissionManagementModal
{
    [Inject] protected IPermissionAppService PermissionAppService { get; set; }

    [Inject] protected IUiMessageService UiMessageService { get; set; }

    protected Modal Modal { get; set; }

    public bool HasAnyResourcePermission { get; set; }
    public bool HasAnyResourceProviderKeyLookupService { get; set; }
    protected string ResourceName { get; set; }
    protected string ResourceKey { get; set; }
    protected string ResourceDisplayName { get; set; }
    protected int PageSize { get; set; } = 10;

    protected Modal CreateModal { get; set; }
    protected Validations CreateValidationsRef { get; set; }
    protected CreateModel CreateEntity { get; set; } = new CreateModel
    {
        Permissions = []
    };
    protected Autocomplete<SearchProviderKeyInfo, string> ProviderKeyAutocompleteRef { get; set; }
    protected Blazorise.Validation ProviderKeyValidationRef { get; set; }
    public GetResourcePermissionDefinitionListResultDto ResourcePermissionDefinitions { get; set; } = new()
    {
        Permissions = []
    };
    protected string CurrentLookupService { get; set; }
    protected string ProviderKey { get; set; }
    protected string ProviderDisplayName { get; set; }
    protected List<ResourceProviderDto> ResourceProviderKeyLookupServices { get; set; } = new();
    protected List<SearchProviderKeyInfo> ProviderKeys { get; set; } = new();
    protected GetResourcePermissionListResultDto ResourcePermissionList = new()
    {
        Permissions = []
    };

    protected Validations EditValidationsRef { get; set; }
    protected Modal EditModal { get; set; }
    protected EditModel EditEntity { get; set; } = new EditModel
    {
        Permissions = []
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

            await Modal.Show();

        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task CloseModal()
    {
        await Modal.Hide();
    }

    protected virtual Task ClosingModal(ModalClosingEventArgs eventArgs)
    {
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;
        return Task.CompletedTask;
    }

    protected virtual async Task OpenCreateModalAsync()
    {
        CurrentLookupService = ResourceProviderKeyLookupServices.FirstOrDefault()?.Name;

        ProviderKey = null;
        ProviderDisplayName = null;
        ProviderKeys = new List<SearchProviderKeyInfo>();
        await ProviderKeyAutocompleteRef.Clear();
        await CreateValidationsRef.ClearAll();

        CreateEntity = new CreateModel
        {
            Permissions = ResourcePermissionDefinitions.Permissions.Select(x => new ResourcePermissionModel
            {
                Name = x.Name,
                DisplayName = x.DisplayName,
                IsGranted = false
            }).ToList()
        };

        await CreateModal.Show();
        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task SelectedProviderKeyAsync(string value)
    {
        ProviderKey = value;
        ProviderDisplayName = ProviderKeys.FirstOrDefault(p => p.ProviderKey == value)?.ProviderDisplayName;

        if (value.IsNullOrWhiteSpace())
        {
            await InvokeAsync(StateHasChanged);
            return;
        }

        var permissionGrants = await PermissionAppService.GetResourceByProviderAsync(ResourceName, ResourceKey, CurrentLookupService, ProviderKey);
        foreach (var permission in CreateEntity.Permissions)
        {
            permission.IsGranted = permissionGrants.Permissions.Any(p => p.Name == permission.Name && p.Providers.Contains(CurrentLookupService) && p.IsGranted);
        }

        await InvokeAsync(StateHasChanged);
    }

    private async Task SearchProviderKeyAsync(AutocompleteReadDataEventArgs autocompleteReadDataEventArgs)
    {
        if (autocompleteReadDataEventArgs.CancellationToken.IsCancellationRequested)
        {
            return;
        }

        if (autocompleteReadDataEventArgs.SearchValue.IsNullOrWhiteSpace())
        {
            ProviderKeys = new List<SearchProviderKeyInfo>();
            return;
        }

        var lookupService = CurrentLookupService;
        var results = (await PermissionAppService.SearchResourceProviderKeyAsync(ResourceName, lookupService, autocompleteReadDataEventArgs.SearchValue, 1)).Keys;

        if (!autocompleteReadDataEventArgs.CancellationToken.IsCancellationRequested)
        {
            ProviderKeys = results;
            await InvokeAsync(StateHasChanged);
        }
    }

    protected virtual async Task OnPermissionCheckedChanged(ResourcePermissionModel permission, bool value)
    {
        permission.IsGranted = value;
        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task GrantAllAsync(bool value)
    {
        foreach (var permission in CreateEntity.Permissions)
        {
            permission.IsGranted = value;
        }

        foreach (var permission in EditEntity.Permissions)
        {
            permission.IsGranted = value;
        }

        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task OpenEditModalAsync(ResourcePermissionGrantInfoDto permission)
    {
        var resourcePermissions = await PermissionAppService.GetResourceByProviderAsync(ResourceName, ResourceKey, permission.ProviderName, permission.ProviderKey);
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

        await EditModal.Show();
    }

    protected virtual Task ClosingCreateModal(ModalClosingEventArgs eventArgs)
    {
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;
        return Task.CompletedTask;
    }

    protected virtual Task ClosingEditModal(ModalClosingEventArgs eventArgs)
    {
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;
        return Task.CompletedTask;
    }

    protected virtual async Task CloseCreateModalAsync()
    {
        await CreateModal.Hide();
    }

    protected virtual async Task CloseEditModalAsync()
    {
        await EditModal.Hide();
    }

    protected virtual async Task OnLookupServiceCheckedValueChanged(string value)
    {
        CurrentLookupService = value;
        ProviderKey = null;
        ProviderDisplayName = null;
        await ProviderKeyAutocompleteRef.Clear();
        await CreateValidationsRef.ClearAll();
        await InvokeAsync(StateHasChanged);
    }

    protected virtual void ValidateProviderKey(ValidatorEventArgs validatorEventArgs)
    {
        validatorEventArgs.Status = ProviderKey.IsNullOrWhiteSpace()
                ? ValidationStatus.Error
                : ValidationStatus.Success;
        validatorEventArgs.ErrorText = L["ThisFieldIsRequired."];
    }

    protected virtual async Task CreateResourcePermissionAsync()
    {
        if (await CreateValidationsRef.ValidateAll())
        {
            await PermissionAppService.UpdateResourceAsync(
                ResourceName,
                ResourceKey,
                new UpdateResourcePermissionsDto
                {
                    ProviderName = CurrentLookupService,
                    ProviderKey = ProviderKey,
                    Permissions = CreateEntity.Permissions.Where(p => p.IsGranted).Select(p => p.Name).ToList()
                }
            );

            await CloseCreateModalAsync();
            ResourcePermissionList = await PermissionAppService.GetResourceAsync(ResourceName, ResourceKey);
            await InvokeAsync(StateHasChanged);
        }
    }

    protected virtual async Task UpdateResourcePermissionAsync()
    {
        if (await EditValidationsRef.ValidateAll())
        {
            await PermissionAppService.UpdateResourceAsync(
                ResourceName,
                ResourceKey,
                new UpdateResourcePermissionsDto
                {
                    ProviderName = EditEntity.ProviderName,
                    ProviderKey = EditEntity.ProviderKey,
                    Permissions = EditEntity.Permissions.Where(p => p.IsGranted).Select(p => p.Name).ToList()
                }
            );

            await CloseEditModalAsync();
            ResourcePermissionList = await PermissionAppService.GetResourceAsync(ResourceName, ResourceKey);
            await InvokeAsync(StateHasChanged);
        }
    }

    protected virtual async Task DeleteResourcePermissionAsync(ResourcePermissionGrantInfoDto permission)
    {
        if(await UiMessageService.Confirm(L["ResourcePermissionDeletionConfirmationMessage"]))
        {
            await PermissionAppService.DeleteResourceAsync(
                ResourceName,
                ResourceKey,
                permission.ProviderName,
                permission.ProviderKey
            );

            ResourcePermissionList = await PermissionAppService.GetResourceAsync(ResourceName, ResourceKey);
            await Notify.Success(L["DeletedSuccessfully"]);
            await InvokeAsync(StateHasChanged);
        }
    }

    public class CreateModel
    {
        public List<ResourcePermissionModel> Permissions { get; set; }
    }

    public class EditModel
    {
        public string ProviderName { get; set; }

        public string ProviderKey { get; set; }

        public List<ResourcePermissionModel> Permissions { get; set; }
    }

    public class ResourcePermissionModel
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool IsGranted { get; set; }
    }
}
