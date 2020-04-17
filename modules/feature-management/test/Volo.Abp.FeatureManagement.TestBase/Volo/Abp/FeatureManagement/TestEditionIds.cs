using System;

namespace Volo.Abp.FeatureManagement
{
    public static class TestEditionIds
    {
        public static Guid Regular { get; }
        public static Guid Enterprise { get; }
        public static Guid Ultimate { get; }

        static TestEditionIds()
        {
            Regular = Guid.Parse("532599ab-c0c0-4345-a04a-e322867b6e15");
            Enterprise = Guid.Parse("27e50758-1feb-436a-be4f-cae8519e0cb2");
            Ultimate = Guid.Parse("6ea78c22-be32-497e-aaba-a2332c564c5e");
        }
    }
}