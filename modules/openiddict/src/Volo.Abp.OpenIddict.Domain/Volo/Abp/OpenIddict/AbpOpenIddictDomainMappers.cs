using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Volo.Abp.OpenIddict.Applications;

namespace Volo.Abp.OpenIddict;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OpenIddictApplicationToOpenIddictApplicationEtoMapper : MapperBase<OpenIddictApplication, OpenIddictApplicationEto>
{
    public override partial OpenIddictApplicationEto Map(OpenIddictApplication source);

    public override partial void Map(OpenIddictApplication source, OpenIddictApplicationEto destination);
}
