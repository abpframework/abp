using System;
using System.Threading.Tasks;

namespace Volo.Abp.EntityFrameworkCore;

public interface IEfCoreDbContextTypeProvider
{
    Type GetDbContextType(Type dbContextType);

    Task<Type> GetDbContextTypeAsync(Type dbContextType);
}
