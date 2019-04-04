using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace BaseManagement
{
    public class BaseTypeDto : EntityDto<Guid>
    {
        public Guid? ParentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Sort { get; set; }
        public string Remark { get; set; }

        public bool HasChildren { get; set; }
    }
}