using System;
using System.Collections.Generic;

namespace Volo.Abp.Data
{
    public class AbpDataFilterOptions
    {
        public bool DefaultFilterState { get; set; } = true;

        public Dictionary<Type, DataFilterState> DefaultStates { get; }

        public AbpDataFilterOptions()
        {
            DefaultStates = new Dictionary<Type, DataFilterState>();
        }
    }
}