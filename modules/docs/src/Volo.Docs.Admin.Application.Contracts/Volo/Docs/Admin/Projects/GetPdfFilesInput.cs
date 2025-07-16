using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Docs.Admin.Projects;

public class GetPdfFilesInput : PagedAndSortedResultRequestDto
{
    public Guid ProjectId { get; set; }
}