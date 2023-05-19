namespace Volo.Abp.VirtualFileSystem;

public class AbpVirtualFileSystemOptions
{
    public VirtualFileSetList FileSets { get; }

    public AbpVirtualFileSystemOptions()
    {
        FileSets = new VirtualFileSetList();
    }
}
