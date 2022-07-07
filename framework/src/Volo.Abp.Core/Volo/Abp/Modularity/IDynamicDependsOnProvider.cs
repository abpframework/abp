using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Volo.Abp.Modularity;

public interface IDynamicDependsOnProvider
{
    public IEnumerable<Type> GetDependencyTypes(IConfiguration config);
}
