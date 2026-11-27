using System;
using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.AgentTools.Sample;

public class BookDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
}

public class CreateBookDto
{
    [Required]
    [StringLength(128)]
    public string Name { get; set; } = default!;
}

public class UpdateBookDto
{
    [Required]
    [StringLength(128)]
    public string Name { get; set; } = default!;
}
