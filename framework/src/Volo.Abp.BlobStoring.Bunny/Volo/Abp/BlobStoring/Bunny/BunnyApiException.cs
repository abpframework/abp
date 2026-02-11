using System;

namespace Volo.Abp.BlobStoring.Bunny;

public class BunnyApiException : Exception
{
    public BunnyApiException(string message)
        : base(message)
    {

    }

    public BunnyApiException(string message, Exception innerException)
        : base(message, innerException)
    {

    }
}
