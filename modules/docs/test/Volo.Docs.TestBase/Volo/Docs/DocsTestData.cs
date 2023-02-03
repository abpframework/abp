﻿using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Docs
{
    public class DocsTestData : ISingletonDependency
    {
        public Guid ProjectId { get; } = Guid.NewGuid();
    }
}
