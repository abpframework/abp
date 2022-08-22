// This file is part of ProjectsAdminClientProxy, you can customize it here
// ReSharper disable once CheckNamespace

using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Docs.Admin.Projects;

namespace Volo.Docs.Admin.ClientProxies;

public partial class ProjectsAdminClientProxy
{
    public async Task<FilterComboboxValuesDto> GetFilterComboboxAsync()
    {
        return await this.RequestAsync<FilterComboboxValuesDto>(nameof(GetFilterComboboxAsync));
    }
}
