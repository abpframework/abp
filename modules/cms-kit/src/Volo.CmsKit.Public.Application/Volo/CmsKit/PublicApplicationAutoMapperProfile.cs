using AutoMapper;
using Volo.Abp.AutoMapper;
using Volo.CmsKit.Comments;

namespace Volo.CmsKit
{
    public class PublicApplicationAutoMapperProfile : Profile
    {
        public PublicApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            CreateMap<Comment, CommentDto>();
            CreateMap<Comment, CommentWithRepliesDto>().Ignore(x=> x.Replies);
        }
    }
}
