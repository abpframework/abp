using System;
using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring.FileSystem
{
    [DependsOn(
        typeof(AbpBlobStoringFileSystemModule)
        )]
    public class AbpBlobStoringFileSystemTestModule : AbpModule
    {
        
    }
}