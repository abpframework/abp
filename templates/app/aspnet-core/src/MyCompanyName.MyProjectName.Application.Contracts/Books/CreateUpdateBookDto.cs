using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyCompanyName.MyProjectName.Books;

public class CreateUpdateBookDto
{

    [Required]
    [StringLength(128)]
    public string Name { get; set; }

    [Required]
    public string BookType { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime PublishDate { get; set; }

    [Required]
    public float Price { get; set; }
    public string Author { get; set; }
}
