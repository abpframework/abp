using System;
using JetBrains.Annotations;

namespace Volo.Abp.Modularity.PlugIns
{
    public class PlugInTypeListSource : IPlugInSource
    {
        private readonly Type[] _moduleTypes;

        public PlugInTypeListSource(params Type[] moduleTypes)
        {
            _moduleTypes = moduleTypes ?? new Type[0];
        }

        [NotNull]
        public Type[] GetModules()
        {
            return _moduleTypes;
        }
    }
}