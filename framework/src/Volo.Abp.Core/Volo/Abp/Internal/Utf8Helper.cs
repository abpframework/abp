using System.IO;
using System.Text;

namespace Volo.Abp.Internal;

public static class Utf8Helper
{
    public static string ReadStringFromStream(Stream stream)
    {
        var bytes = stream.GetAllBytes();
        var skipCount = HasBom(bytes) ? 3 : 0;
        return Encoding.UTF8.GetString(bytes, skipCount, bytes.Length - skipCount);
    }

    private static bool HasBom(byte[] bytes)
    {
        if (bytes.Length < 3)
        {
            return false;
        }

        if (!(bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF))
        {
            return false;
        }

        return true;
    }
}
