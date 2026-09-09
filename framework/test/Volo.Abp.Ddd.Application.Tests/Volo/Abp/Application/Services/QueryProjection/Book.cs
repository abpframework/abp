using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Application.Services.QueryProjection;

public class Book : Entity<Guid>
{
    public string Name { get; set; } = default!;

    public int Price { get; set; }

    public Book()
    {

    }

    public Book(Guid id, string name, int price)
        : base(id)
    {
        Name = name;
        Price = price;
    }
}
