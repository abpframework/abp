using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace AbpDesk.Blogging
{
    public class BlogPost : AggregateRoot
    {
        public virtual string Title { get; protected set; }

        public virtual string Body { get; protected set; }

        public virtual ICollection<BlogPostComment> Comments { get; protected set; }

        protected BlogPost()
        {

        }

        public BlogPost(Guid id, [NotNull] string title, [NotNull] string body)
        {
            Check.NotNull(title, nameof(title));
            Check.NotNull(body, nameof(body));

            Id = id;
            Title = title;
            Body = body;

            Comments = new List<BlogPostComment>();
        }

        public void SetTitle([NotNull] string title)
        {
            Check.NotNull(title, nameof(title));

            Title = title;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Title = {Title}, Body = {Body.TruncateWithPostfix(32)}";
        }
    }
}
