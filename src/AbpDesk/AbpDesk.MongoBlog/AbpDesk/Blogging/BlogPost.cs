using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo;
using Volo.Abp.Domain.Entities;
using Volo.ExtensionMethods;

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

        public BlogPost([NotNull] string title, [NotNull] string body)
        {
            Id = Guid.NewGuid().ToString("D");

            Check.NotNull(title, nameof(title));
            Check.NotNull(body, nameof(body));

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
