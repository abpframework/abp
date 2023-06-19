namespace Volo.Abp.Imaging;

public enum ImageProcessState : byte
{
    Done = 1,
    Canceled = 2,
    Unsupported = 3,
}