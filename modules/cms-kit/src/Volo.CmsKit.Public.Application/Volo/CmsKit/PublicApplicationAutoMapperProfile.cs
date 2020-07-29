using AutoMapper;
using Volo.Abp.AutoMapper;
using Volo.CmsKit.Comments;
using Volo.CmsKit.Users;

namespace Volo.CmsKit
{
    public class PublicApplicationAutoMapperProfile : Profile
    {
        public PublicApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            CreateMap<CmsUser, CmsUserDto>();
            CreateMap<Comment, CommentDto>().Ignore(x=> x.Author);
            CreateMap<Comment, CommentWithDetailsDto>().Ignore(x=> x.Replies).Ignore(x=> x.Author);
        }
    }
}
