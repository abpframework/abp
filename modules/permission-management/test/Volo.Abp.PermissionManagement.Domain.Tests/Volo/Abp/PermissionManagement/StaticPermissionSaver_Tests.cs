namespace Volo.Abp.PermissionManagement;

public class StaticPermissionSaver_Tests : PermissionTestBase
{
    private readonly IStaticPermissionSaver _saver;

    public StaticPermissionSaver_Tests()
    {
        _saver = GetRequiredService<IStaticPermissionSaver>();
    }


}
