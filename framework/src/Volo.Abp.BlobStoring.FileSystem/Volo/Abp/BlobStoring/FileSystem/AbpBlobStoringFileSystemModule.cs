using System;
using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring.FileSystem
{
    [DependsOn(
        typeof(AbpBlobStoringModule)
        )]
    public class AbpBlobStoringFileSystemModule : AbpModule
    {
        
    }
}