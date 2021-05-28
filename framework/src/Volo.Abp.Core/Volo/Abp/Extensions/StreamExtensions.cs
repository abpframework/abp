using System.IO;

public static class StreamExtensions
{
    public static long? GetNullableLength(this Stream stream)
    {
        try
        {
            return stream?.Length;
        }
        catch
        {
            /*some stream classes throw exceptions when accessing Length because they do not have access to such information */
            return null;
        }
    }
    public static long? GetNullablePosition(this Stream stream)
    {
        try
        {
            return stream?.Position;
        }
        catch
        {
            /*some stream classes throw exceptions when accessing Position because they do not have access to such information */
            return null;
        }
    }
}
