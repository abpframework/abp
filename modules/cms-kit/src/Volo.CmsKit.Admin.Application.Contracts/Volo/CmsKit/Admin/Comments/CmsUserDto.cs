using System;

namespace Volo.CmsKit.Admin.Comments
{
    [Serializable]
    public class CmsUserDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
    }
}