namespace Volo.Abp.Image;

public class ImageProcessResult<T>
{
    public T Result { get; }
    public bool IsSuccess { get; }
    
    public ImageProcessResult(T result, bool isSuccess)
    {
        Result = result;
        IsSuccess = isSuccess;
    }
}