using System;
using Microsoft.AspNetCore.Http;

namespace Volo.Abp.Storage.Configuration
{
    public class FileSystemStorageServerOptions
    {
        public Uri BaseUri { get; set; }

        public PathString EndpointPath { get; set; } = "/.well-known/storage";

        public byte[] SigningKey { get; set; }
    }
}
