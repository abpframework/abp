using System;

namespace Volo.Abp.BlobStoring;

public class BlobAlreadyExistsException : AbpException
{
    public BlobAlreadyExistsException()
    {

    }

    public BlobAlreadyExistsException(string message)
        : base(message)
    {

    }

    public BlobAlreadyExistsException(string message, Exception innerException)
        : base(message, innerException)
    {

    }
}
