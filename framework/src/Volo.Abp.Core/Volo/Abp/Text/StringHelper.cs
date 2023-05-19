using System.Text;

namespace Volo.Abp.Text;

public class StringHelper
{
    /// <summary>
    /// Converts a byte[] to string without BOM (byte order mark).
    /// </summary>
    /// <param name="bytes">The byte[] to be converted to string</param>
    /// <param name="encoding">The encoding to get string. Default is UTF8</param>
    /// <returns></returns>
    public static string ConvertFromBytesWithoutBom(byte[] bytes, Encoding encoding = null)
    {
        if (bytes == null)
        {
            return null;
        }

        if (encoding == null)
        {
            encoding = Encoding.UTF8;
        }

        var hasBom = bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF;

        if (hasBom)
        {
            return encoding.GetString(bytes, 3, bytes.Length - 3);
        }
        else
        {
            return encoding.GetString(bytes);
        }
    }
}
