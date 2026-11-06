namespace Volo.CmsKit.RobotsTxts;

public static class RobotsTxtConsts
{
    /// <summary>
    /// Default value: 500 KiB, the maximum size Google parses for a robots.txt file.
    /// </summary>
    public static int MaxContentLength { get; set; } = 500 * 1024;
}
