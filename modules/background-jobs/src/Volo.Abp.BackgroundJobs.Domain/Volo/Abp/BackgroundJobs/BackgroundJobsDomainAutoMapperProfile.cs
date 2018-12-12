using AutoMapper;
using Volo.Abp.AutoMapper;

namespace Volo.Abp.BackgroundJobs
{
    public class BackgroundJobsDomainAutoMapperProfile : Profile
    {
        public BackgroundJobsDomainAutoMapperProfile()
        {
            CreateMap<BackgroundJobInfo, BackgroundJobRecord>()
                .Ignore(record => record.ConcurrencyStamp)
                .Ignore(record => record.ExtraProperties)
                .ReverseMap();
        }
    }
}
