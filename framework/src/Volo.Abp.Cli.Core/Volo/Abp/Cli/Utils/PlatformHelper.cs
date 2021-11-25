using System;
using System.Runtime.InteropServices;

namespace Volo.Abp.Cli.Utils;

public class PlatformHelper
{
    public static OSPlatform GetOperatingSystem()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            //MAC
            return OSPlatform.OSX;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return OSPlatform.Linux;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return OSPlatform.Windows;
        }

        throw new Exception("Cannot determine operating system!");
    }

    public static RuntimePlatform GetPlatform()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ||
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return RuntimePlatform.LinuxOrMacOs;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return RuntimePlatform.Windows;
        }

        throw new Exception("Cannot determine runtime platform!");
    }
}

public enum RuntimePlatform
{
    Windows = 1,
    LinuxOrMacOs = 2
}
