using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Extensibility;
using Volo.Abp.AspNetCore.Components.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Extensibility.TableColumns;
using Volo.Abp.Identity.Localization;
using Volo.Abp.PermissionManagement.Blazor.Components;

namespace Volo.Abp.Identity.Blazor.Pages.Identity
{
    public partial class RoleManagement
    {
        protected const string PermissionProviderName = "R";

        protected PermissionManagementModal PermissionManagementModal;

        protected string ManagePermissionsPolicyName;

        protected bool HasManagePermissionsPermission { get; set; }

        public RoleManagement()
        {
            ObjectMapperContext = typeof(AbpIdentityBlazorModule);
            LocalizationResource = typeof(IdentityResource);

            CreatePolicyName = IdentityPermissions.Roles.Create;
            UpdatePolicyName = IdentityPermissions.Roles.Update;
            DeletePolicyName = IdentityPermissions.Roles.Delete;
            ManagePermissionsPolicyName = IdentityPermissions.Roles.ManagePermissions;
        }

        protected override ValueTask SetEntityActionsAsync()
        {
            UIExtensions.Instance
                .EntityActions
                .Get<RoleManagement>()
                .AddIfNotContains(new EntityAction[]
                {
                    new EntityAction
                    {
                        Text = L["Edit"],
                        RequiredPolicy = UpdatePolicyName,
                        Clicked = async (data) => await OpenEditModalAsync(data.As<IdentityRoleDto>())
                    },
                    new EntityAction
                    {
                        Text = L["Permissions"],
                        RequiredPolicy = ManagePermissionsPolicyName,
                        Clicked = async (data) =>
                        {
                            await PermissionManagementModal.OpenAsync(PermissionProviderName,
                                data.As<IdentityRoleDto>().Name);
                        }
                    },
                    new EntityAction
                    {
                        Text = L["Delete"],
                        RequiredPolicy = DeletePolicyName,
                        Clicked = async (data) => await DeleteEntityAsync(data.As<IdentityRoleDto>()),
                        ConfirmationMessage = (data) => GetDeleteConfirmationMessage(data.As<IdentityRoleDto>())
                    }
                });

            return ValueTask.CompletedTask;
        }

        protected override ValueTask SetTableColumnsAsync()
        {
            UIExtensions.Instance
                .TableColumns
                .Get<RoleManagement>()
                .AddIfNotContains(new TableColumn[]
                {
                    new TableColumn
                    {
                        Title = L["Actions"],
                        Actions = UIExtensions.Instance.EntityActions.Get<RoleManagement>()
                    },
                    new TableColumn
                    {
                        Title = L["RoleName"],
                        Data = nameof(IdentityRoleDto.Name),
                        Render = (data) =>
                        {
                            return (builder) =>
                            {
                                var role = data.As<IdentityRoleDto>();
                                builder.AddContent(0, role.Name);

                                if (role.IsDefault)
                                {
                                    builder.OpenComponent<Badge>();
                                    builder.AddAttribute(0, "Color", Color.Primary);
                                    builder.AddAttribute(1, "Margin", Margin.Is1.FromLeft);
                                    builder.AddAttribute(2, "ChildContent",
                                        (RenderFragment) ((builder2) =>
                                        {
                                            builder2.AddContent(3, L["DisplayName:IsDefault"]);
                                        }));
                                    builder.CloseComponent();
                                }

                                if (role.IsPublic)
                                {
                                    builder.OpenComponent<Badge>();
                                    builder.AddAttribute(0, "Color", Color.Light);
                                    builder.AddAttribute(1, "Margin", Margin.Is1.FromLeft);
                                    builder.AddAttribute(2, "ChildContent",
                                        (RenderFragment) ((builder2) =>
                                        {
                                            builder2.AddContent(3, L["DisplayName:IsPublic"]);
                                        }));
                                    builder.CloseComponent();
                                }
                            };
                        }
                    },
                });
            
            return ValueTask.CompletedTask;
        }
        
        protected override async Task SetPermissionsAsync()
        {
            await base.SetPermissionsAsync();

            HasManagePermissionsPermission = await AuthorizationService.IsGrantedAsync(IdentityPermissions.Roles.ManagePermissions);
        }

        protected override string GetDeleteConfirmationMessage(IdentityRoleDto entity)
        {
            return string.Format(L["RoleDeletionConfirmationMessage"], entity.Name);
        }
    }
}
