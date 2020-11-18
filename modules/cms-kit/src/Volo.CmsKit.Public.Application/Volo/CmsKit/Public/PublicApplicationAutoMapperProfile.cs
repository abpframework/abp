using AutoMapper;
using Volo.Abp.AutoMapper;
using Volo.CmsKit.Comments;
using Volo.CmsKit.Public.Comments;
using Volo.CmsKit.Public.Ratings;
using Volo.CmsKit.Ratings;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.Public
{
    public class PublicApplicationAutoMapperProfile : Profile
    {
        public PublicApplicationAutoMapperProfile()
        {
            CreateMap<CmsUser, CmsUserDto>();

            CreateMap<Comment, CommentDto>()
                .Ignore(x=> x.Author);

            CreateMap<Comment, CommentWithDetailsDto>()
                .Ignore(x=> x.Replies)
                .Ignore(x=> x.Author);

            CreateMap<Rating, RatingDto>();
        }
    }
}
