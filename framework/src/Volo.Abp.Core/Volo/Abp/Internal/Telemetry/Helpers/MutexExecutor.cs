using System;
using System.IO;
using System.Threading;

namespace Volo.Abp.Internal.Telemetry.Helpers;

static internal class MutexExecutor
{
    private const string MutexName = "Global\\MyFileReadMutex";
    private const int TimeoutMilliseconds = 3000;

    public static string? ReadFileSafely(string filePath)
    {
        using var mutex = new Mutex(false, MutexName);

        if (!mutex.WaitOne(TimeoutMilliseconds))
        {
            return null;
        }

        try
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            return File.ReadAllText(filePath);
        }
        catch (IOException)
        {
            return null;
        }
        finally
        {
            try
            {
                mutex.ReleaseMutex();
            }
            catch
            {
                // Already released or abandoned
            }
        }
    }
}