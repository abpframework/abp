using System.Runtime.InteropServices;
using Volo.Abp.DependencyInjection;

namespace System.Runtime;

public class OSPlatformProvider : IOSPlatformProvider, ITransientDependency
{
    public virtual OSPlatform GetCurrentOSPlatform()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return OSPlatform.OSX; //MAC
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return OSPlatform.Windows;
        }

        return OSPlatform.Linux;
    }
}
