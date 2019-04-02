using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace BaseManagement
{
    public class UpdateBaseTypeDto:Entity<Guid>
    {
        public Guid? ParentId { get; set; }
        [Required]
        [MaxLength(BaseConsts.MaxCodeLength)]
        public string Code { get; set; }
        [Required]
        [MaxLength(BaseConsts.MaxNameLength)]
        public string Name { get; set; }
        public int Sort { get; set; }
        [MaxLength(BaseConsts.MaxRemarkLength)]
        public string Remark { get; set; }
    }
}