using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Services;

namespace Volo.Docs.GitHub.Documents
{
    public interface IGithubPatchAnalyzer : IDomainService
    {
        bool HasPatchSignificantChanges(string patch);
    }
}
