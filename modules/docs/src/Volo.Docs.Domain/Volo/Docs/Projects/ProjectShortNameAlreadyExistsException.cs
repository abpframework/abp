using Volo.Abp;

namespace Volo.Docs.Projects
{
    public class ProjectShortNameAlreadyExistsException : BusinessException
    {
        public ProjectShortNameAlreadyExistsException(string shortName)
            : base("Volo.Docs.Domain:010002")
        {
            WithData("ShortName", shortName);
        }
    }
}