using System;
using System.Threading.Tasks;

namespace Volo.Abp.SecurityLog;

public interface ISecurityLogManager
{
    Task SaveAsync(Action<SecurityLogInfo> saveAction = null);
}
