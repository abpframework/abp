using System;
using Microsoft.AspNetCore.Http;

namespace Volo.Abp.Storage.FileSystem.Server
{
    public class AbpFileSystemStorageServerOptions
    {
        public Uri BaseUri { get; set; }

        public PathString EndpointPath { get; set; } = "/.well-known/storage";

        public byte[] SigningKey { get; set; }
    }
}