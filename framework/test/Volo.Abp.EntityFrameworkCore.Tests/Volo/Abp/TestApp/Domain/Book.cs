using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.TestApp.Domain
{
    public class Book : Entity<Guid>, ISoftDelete
    {
        public Guid AuthorId { get; set; }
        
        public string Title { get; set; }

        private Book()
        {

        }
        
        public Book(Guid authorId, Guid id, string title)
        {
            Id = id;
            AuthorId = authorId;
            Title = title;
        }

        public bool IsDeleted { get; set; }
    }
}