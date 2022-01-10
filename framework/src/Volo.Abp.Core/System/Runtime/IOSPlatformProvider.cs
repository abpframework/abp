using System.Runtime.InteropServices;

namespace System.Runtime;

public interface IOSPlatformProvider
{
    OSPlatform GetCurrentOSPlatform();
}
