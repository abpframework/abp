using System;
using Volo.Abp.Domain.Repositories;

namespace Volo.CmsKit.MediaDescriptors;

public interface IMediaDescriptorRepository : IBasicRepository<MediaDescriptor, Guid>
{

}
