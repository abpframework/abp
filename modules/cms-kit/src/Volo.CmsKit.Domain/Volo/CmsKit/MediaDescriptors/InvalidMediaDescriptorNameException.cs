using JetBrains.Annotations;
using Volo.Abp;

namespace Volo.CmsKit.MediaDescriptors;

public class InvalidMediaDescriptorNameException : BusinessException
{
    public InvalidMediaDescriptorNameException([NotNull] string name)
    {
        Code = CmsKitErrorCodes.MediaDescriptors.InvalidName;
        WithData(nameof(MediaDescriptor.Name), name);
    }
}
