﻿using System.Threading.Tasks;

 namespace Volo.Abp.Account.Web.ProfileManagement
{
    public interface IProfileManagementPageContributor
    {
        Task ConfigureAsync(ProfileManagementPageCreationContext context);
    }
}
