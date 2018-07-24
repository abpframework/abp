using AutoMapper;

namespace Volo.Abp.BackgroundJobs
{
    public class BackgroundJobsDomainAutoMapperProfile : Profile
    {
        public BackgroundJobsDomainAutoMapperProfile()
        {
            CreateMap<BackgroundJobInfo, BackgroundJobRecord>()
                .ReverseMap();
        }
    }
}
