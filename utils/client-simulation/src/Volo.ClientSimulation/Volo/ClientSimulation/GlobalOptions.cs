using Volo.Abp.DependencyInjection;

namespace Volo.ClientSimulation
{
    public class GlobalOptions : ISingletonDependency
    {
        public int MaxClientCount { get; set; } = 2;

        public GlobalOptions Clone()
        {
            return new GlobalOptions
            {
                MaxClientCount = MaxClientCount
            };
        }
    }
}