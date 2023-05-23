namespace Volo.Abp.Imaging;

public class ImageContributorResult<T>
{
    public T Result { get; }
    public bool IsSupported { get; }
    
    public ImageContributorResult(T result, bool isSupported = true)
    {
        Result = result;
        IsSupported = isSupported;
    }
}