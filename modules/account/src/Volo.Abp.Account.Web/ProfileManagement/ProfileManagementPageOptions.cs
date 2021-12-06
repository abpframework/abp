﻿using System.Collections.Generic;

namespace Volo.Abp.Account.Web.ProfileManagement;

public class ProfileManagementPageOptions
{
    public List<IProfileManagementPageContributor> Contributors { get; }

    public ProfileManagementPageOptions()
    {
        Contributors = new List<IProfileManagementPageContributor>();
    }
}
