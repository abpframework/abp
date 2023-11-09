using AutoMapper;
using Volo.Abp.AutoMapper;
using Volo.CmsKit.Menus;
using Volo.CmsKit.Public.Comments;

namespace Volo.CmsKit.Public.Web;

public class CmsKitPublicWebAutoMapperProfile : Profile
{
    public CmsKitPublicWebAutoMapperProfile()
    {
        CreateMap<CreateCommentWithParametersInput, CreateCommentInput>()
            .Ignore(x=> x.ExtraProperties);
    }
}
