using System.Linq;
using Volo.Docs.Projects;

namespace Volo.Docs.Documents
{
    public static class DocumentVersionHelper
    {
        public static bool IsValidUnlistedVersionName(string version)
        {
            return !string.IsNullOrEmpty(version) && version.Length <= ProjectConsts.MaxVersionNameLength &&
                   version.All(c => c is >= 'a' and <= 'z' or >= 'A' and <= 'Z' or >= '0' and <= '9' or '.' or '-' or '_' or '+');
        }
    }
}
