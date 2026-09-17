using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.Abp.TestApp.Domain;

public class Blog : FullAuditedAggregateRoot<Guid>
{
    public Blog(Guid id)
        : base(id)
    {
    }

    public string Name { get; set; }

    public List<BlogPost> BlogPosts { get; set; }
}

public class BlogPost : Entity<Guid>
{
    public BlogPost(Guid id)
        : base(id)
    {
    }

    public Guid BlogId { get; set; }
    public Blog Blog { get; set; }

    public string Title { get; set; }
}
