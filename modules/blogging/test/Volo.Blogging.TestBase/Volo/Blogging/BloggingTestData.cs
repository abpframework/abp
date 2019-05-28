using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Blogging
{
    public class BloggingTestData : ISingletonDependency
    {
        public Guid Blog1Id { get; } = Guid.NewGuid();
        public Guid Blog1Post1Id { get; } = Guid.NewGuid();
        public Guid Blog1Post2Id { get; } = Guid.NewGuid();
        public Guid Blog1Post1Comment1Id { get; } = Guid.NewGuid();
        public Guid Blog1Post1Comment2Id { get; } = Guid.NewGuid();
        public string Tag1Name { get; } = "Tag1Name";
        public string Tag2Name { get; } = "Tag2Name";
    }
}