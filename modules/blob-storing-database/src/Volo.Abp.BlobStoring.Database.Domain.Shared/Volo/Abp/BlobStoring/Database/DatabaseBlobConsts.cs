namespace Volo.Abp.BlobStoring.Database;

public static class DatabaseBlobConsts
{
    /// <summary>
    /// Default value: 256.
    /// </summary>
    public static int MaxNameLength { get; set; } = 256;

    /// <summary>
    /// Default value: <see cref="int.MaxValue"/> (2GB).
    /// </summary>
    public static int MaxContentLength { get; set; } = int.MaxValue;
}
