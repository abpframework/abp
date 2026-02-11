using System.Threading.Tasks;

namespace Volo.Abp.SettingManagement;

public interface IStaticSettingSaver
{
    Task SaveAsync();
}
