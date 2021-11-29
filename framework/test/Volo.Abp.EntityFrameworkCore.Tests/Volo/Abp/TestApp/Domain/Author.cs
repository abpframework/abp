using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.TestApp.Domain;

public class Author : AggregateRoot<Guid>
{
    public string Name { get; set; }

    public ICollection<Book> Books { get; set; }

    private Author()
    {

    }

    public Author(Guid id, string name)
        : base(id)
    {
        Name = name;
        Books = new List<Book>();
    }
}
