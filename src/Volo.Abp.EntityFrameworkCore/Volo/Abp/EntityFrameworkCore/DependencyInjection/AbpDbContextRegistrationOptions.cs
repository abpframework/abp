using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EntityFrameworkCore.DependencyInjection
{
    public class AbpDbContextRegistrationOptions : CommonDbContextRegistrationOptions, IAbpDbContextRegistrationOptionsBuilder
    {
        public AbpDbContextRegistrationOptions(Type originalDbContextType)
            : base(originalDbContextType)
        {

        }
    }
}