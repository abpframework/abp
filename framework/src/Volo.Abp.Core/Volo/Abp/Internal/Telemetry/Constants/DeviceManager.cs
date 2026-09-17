using System;

namespace Volo.Abp.Internal.Telemetry.Constants;

static internal class DeviceManager
{
    public static string GetUniquePhysicalKey(bool shouldHash)
    {
        char platformId = '?';
        char osArchitecture = '?';
        string operatingSystem = "?";

        try
        {
            string osPrefix;
            string uniqueKey;

            platformId = GetPlatformIdOrDefault();
            osArchitecture = GetOsArchitectureOrDefault();

            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform
                    .Windows))
            {
                operatingSystem = "Windows";
                uniqueKey = GetUniqueKeyForWindows();
                osPrefix = "W";
            }
            else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices
                         .OSPlatform.Linux))
            {
                operatingSystem = "Linux";
                uniqueKey = GetHarddiskSerialForLinux();
                osPrefix = "L";
            }
            else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices
                         .OSPlatform.OSX)) //MAC
            {
                operatingSystem = "OSX";
                uniqueKey = GetHarddiskSerialForOsX();
                osPrefix = "O";
            }
            else
            {
                operatingSystem = "Other";
                uniqueKey = GetNetworkAdapterSerial();
                osPrefix = "X";
            }

            if (shouldHash)
            {
                uniqueKey = ConvertToMd5(uniqueKey).ToUpperInvariant();
            }

            return osPrefix + platformId + osArchitecture + "-" + uniqueKey;
        }
        catch
        {
            return Guid.NewGuid().ToString();
        }
    }

    private static string GetNetworkAdapterSerial()
    {
        string macAddress = string.Empty;

        var networkInterfaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
        foreach (var networkInterface in networkInterfaces)
        {
            if (networkInterface.NetworkInterfaceType == System.Net.NetworkInformation.NetworkInterfaceType.Loopback)
            {
                continue;
            }

            var physicalAddress = networkInterface.GetPhysicalAddress().ToString();
            if (string.IsNullOrEmpty(physicalAddress))
            {
                continue;
            }

            macAddress = physicalAddress;
            break;
        }

        return macAddress!;
    }

    private static char GetPlatformIdOrDefault(char defaultValue = '*')
    {
        try
        {
            return ((int)System.Environment.OSVersion.Platform).ToString()[0];
        }
        catch
        {
            return defaultValue;
        }
    }

    private static string ConvertToMd5(string text)
    {
        using (var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider())
        {
            return EncodeBase64(md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(text)));
        }
    }

    private static string EncodeBase64(byte[] ba)
    {
        var hex = new System.Text.StringBuilder(ba.Length * 2);

        foreach (var b in ba)
        {
            hex.AppendFormat("{0:x2}", b);
        }

        return hex.ToString();
    }


    private static char GetOsArchitectureOrDefault(char defaultValue = '*')
    {
        try
        {
            return ((int)System.Runtime.InteropServices.RuntimeInformation.OSArchitecture).ToString()[0];
        }
        catch
        {
            return defaultValue;
        }
    }

    private static string GetUniqueKeyForWindows()
    {
        try
        {
            return GetBaseBoardSerialNumberForWindows();
        }
        catch
        {
        }

        return GetWindowsMachineUniqueId();
    }

    private static string GetBaseBoardSerialNumberForWindows()
    {
        using (var managementObjectSearcher =
               new System.Management.ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard"))
        {
            using (var searcherObj = managementObjectSearcher.Get())
            {
                if (searcherObj.Count == 0)
                {
                    throw new System.Exception("No unique computer ID found for this computer!");
                }

                var managementObjectEnumerator = searcherObj.GetEnumerator();
                managementObjectEnumerator.MoveNext();
                return managementObjectEnumerator.Current.GetPropertyValue("SerialNumber").ToString()!;
            }
        }
    }

    private static string GetWindowsMachineUniqueId()
    {
        return RunCommandAndGetOutput("powershell (Get-CimInstance -Class Win32_ComputerSystemProduct).UUID");
    }


    private static string GetHarddiskSerialForLinux()
    {
        return RunCommandAndGetOutput(
            "udevadm info --query=all --name=/dev/sda | grep ID_SERIAL_SHORT | tr -d \"ID_SERIAL_SHORT=:\"");
    }

    private static string GetHarddiskSerialForOsX()
    {
        var command =
            "ioreg -rd1 -c IOPlatformExpertDevice | awk '/IOPlatformUUID/ { split($0, line, \"\\\"\"); printf(\"%s\\n\", line[4]); }'";

        command = System.Text.RegularExpressions.Regex.Replace(command, @"(\\*)" + "\"", @"$1$1\" + "\"");

        return RunCommandAndGetOutput(command);
    }

    private static string RunCommandAndGetOutput(string command)
    {
        var output = "";

        using (var process = new System.Diagnostics.Process())
        {
            process.StartInfo = new System.Diagnostics.ProcessStartInfo(GetFileName())
            {
                Arguments = GetArguments(command),
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            process.Start();
            process?.WaitForExit();

            using (var stdOut = process!.StandardOutput)
            {
                using (var stdErr = process.StandardError)
                {
                    output = stdOut.ReadToEnd();
                    output += stdErr.ReadToEnd();
                }
            }
        }

        return output.Trim();
    }

    private static string GetFileName()
    {
        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(
                System.Runtime.InteropServices.OSPlatform.OSX) ||
            System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform
                .Linux))
        {
            string[] fileNames = { "/bin/bash", "/usr/bin/bash", "/bin/sh", "/usr/bin/sh" };
            foreach (var fileName in fileNames)
            {
                try
                {
                    if (System.IO.File.Exists(fileName))
                    {
                        return fileName;
                    }
                }
                catch
                {
                    //ignore
                }
            }

            return "/bin/bash";
        }

        //Windows default.
        return "cmd.exe";
    }

    private static string GetArguments(string command)
    {
        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(
                System.Runtime.InteropServices.OSPlatform.OSX) ||
            System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform
                .Linux))
        {
            return "-c \"" + command + "\"";
        }

        //Windows default.
        return "/C \"" + command + "\"";
    }
}