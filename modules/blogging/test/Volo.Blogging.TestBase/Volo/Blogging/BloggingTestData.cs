using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Blogging
{
    public class BloggingTestData : ISingletonDependency
    {
        public Guid Blog1Id { get; } = Guid.NewGuid();
        public Guid Blog1Post1Id { get; } = Guid.NewGuid();
        public Guid Blog1Post2Id { get; } = Guid.NewGuid();
    }
}