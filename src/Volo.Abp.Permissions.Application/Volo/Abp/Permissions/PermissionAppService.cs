using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.Permissions
{
    public class PermissionAppService : ApplicationService, IPermissionAppService
    {
        private readonly IPermissionManager _permissionManager;

        public PermissionAppService(IPermissionManager permissionManager)
        {
            _permissionManager = permissionManager;
        }

        public async Task<PermissionGrantInfoDto> GetAsync(string name, string providerName, string providerKey)
        {
            throw new NotImplementedException();
        }

        public async Task<GetPermissionListResultDto> GetListAsync(string providerName, string providerKey)
        {
            return new GetPermissionListResultDto
            {
                Groups = new List<PermissionGroupDto>
                {
                    new PermissionGroupDto
                    {
                        Name = "Volo.Calendar",
                        DisplayName = "Calendar",
                        Permissions = new List<PermissionGrantInfoDto>
                        {
                            new PermissionGrantInfoDto
                            {
                                Name = "CalendarManagement",
                                DisplayName = "Calendar management",
                                ParentName = null,
                                IsGranted = true,
                                Providers = new List<ProviderInfoDto>
                                {
                                    new ProviderInfoDto
                                    {
                                        ProviderKey = "Role",
                                        ProviderName = "Moderator"
                                    },
                                    new ProviderInfoDto
                                    {
                                        ProviderKey = "OU",
                                        ProviderName = "44238045-6f18-44cd-8f1f-4e2d61ef1c81"
                                    }
                                }
                            },
                            new PermissionGrantInfoDto
                            {
                                Name = "CreateNewCalendar",
                                DisplayName = "Create new calendar",
                                ParentName = "CalendarManagement",
                                IsGranted = true,
                                Providers = new List<ProviderInfoDto>
                                {
                                    new ProviderInfoDto
                                    {
                                        ProviderKey = "User",
                                        ProviderName = "bb3c2a2f-118d-4d2e-b1d5-e07f3b0a917c"
                                    }
                                }
                            },
                            new PermissionGrantInfoDto
                            {
                                Name = "DeleteExistingCalendars",
                                DisplayName = "Delete existing calendars",
                                ParentName = "CalendarManagement",
                                IsGranted = false
                            }
                        }
                    },
                    new PermissionGroupDto
                    {
                        Name = "Volo.Identity",
                        DisplayName = "Identity",
                        Permissions = new List<PermissionGrantInfoDto>
                        {
                            new PermissionGrantInfoDto
                            {
                                Name = "UserManagement",
                                DisplayName = "User management",
                                ParentName = null,
                                IsGranted = true,
                                Providers = new List<ProviderInfoDto>
                                {
                                    new ProviderInfoDto
                                    {
                                        ProviderKey = "Role",
                                        ProviderName = "Administrator"
                                    }
                                }
                            },
                            new PermissionGrantInfoDto
                            {
                                Name = "CreateNewUser",
                                DisplayName = "Create new user",
                                ParentName = "UserManagement",
                                IsGranted = true,
                                Providers = new List<ProviderInfoDto>
                                {
                                    new ProviderInfoDto
                                    {
                                        ProviderKey = "User",
                                        ProviderName = "bb3c2a2f-118d-4d2e-b1d5-e07f3b0a917c"
                                    }
                                }
                            },
                            new PermissionGrantInfoDto
                            {
                                Name = "DeleteExistingUsers",
                                DisplayName = "Delete existing users",
                                ParentName = "UserManagement",
                                IsGranted = false
                            }
                        }
                    }
                }
            };
        }

        public async Task UpdateAsync(string providerName, string providerKey, UpdatePermissionsDto input)
        {
            
        }
    }
}
