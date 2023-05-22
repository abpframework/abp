using System;

namespace Volo.Abp.Imaging;

public class ImageContributorResult<T>
{
    public T Result { get; }
    public bool IsSuccess { get; }
    public bool IsSupported { get; }
    public Exception Exception { get; }
    
    public ImageContributorResult(T result, bool isSuccess, bool isSupported = true, Exception exception = null)
    {
        Result = result;
        IsSuccess = isSuccess;
        IsSupported = isSupported;
        Exception = exception;
    }
}