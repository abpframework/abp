using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace  Volo.Abp.BackgroundJobs;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class BackgroundJobInfoToBackgroundJobRecordMapper
    : MapperBase<BackgroundJobInfo, BackgroundJobRecord>
{
    [MapperIgnoreTarget(nameof(BackgroundJobRecord.ConcurrencyStamp))]
    [MapperIgnoreTarget(nameof(BackgroundJobRecord.ExtraProperties))]
    public override partial BackgroundJobRecord Map(BackgroundJobInfo source);
    
    [MapperIgnoreTarget(nameof(BackgroundJobRecord.ConcurrencyStamp))]
    [MapperIgnoreTarget(nameof(BackgroundJobRecord.ExtraProperties))]
    public override partial void Map(BackgroundJobInfo source, BackgroundJobRecord destination);

    [ObjectFactory]
    protected BackgroundJobRecord CreateBackgroundJobRecord(BackgroundJobInfo source)
    {
        return new BackgroundJobRecord(source.Id);
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class BackgroundJobRecordToBackgroundJobInfoMapper
    : MapperBase<BackgroundJobRecord, BackgroundJobInfo>
{
    public override partial BackgroundJobInfo Map(BackgroundJobRecord source);
    
    public override partial void Map(BackgroundJobRecord source, BackgroundJobInfo destination);
}
