using System;
using Microsoft.EntityFrameworkCore.Storage;

namespace Volo.Abp.EntityFrameworkCore.MySQL;

/* MySql.EntityFrameworkCore (up to 10.0.9) maps Guid[] query parameters to its
 * scalar GUID mapping and throws NullReferenceException at parameter binding.
 * This plugin runs before the provider's own lookup and returns the collection
 * mapping the provider already builds for List<Guid>. Remove once the provider
 * handles Guid[] parameters. */
internal sealed class MySQLGuidArrayTypeMappingSourcePlugin : IRelationalTypeMappingSourcePlugin
{
    public RelationalTypeMapping? FindMapping(in RelationalTypeMappingInfo mappingInfo)
    {
        if (mappingInfo.ClrType == typeof(Guid[]) && mappingInfo.ElementTypeMapping is not null)
        {
            return new GuidArrayTypeMapping(mappingInfo.ElementTypeMapping);
        }

        return null;
    }

    /* RelationalTypeMapping.Clone no longer takes a clrType since EF Core 11, so the
     * CLR type has to come from the mapping parameters instead. */
    private sealed class GuidArrayTypeMapping : StringTypeMapping
    {
        public GuidArrayTypeMapping(CoreTypeMapping elementMapping)
            : base(new RelationalTypeMappingParameters(
                new CoreTypeMappingParameters(typeof(Guid[]), elementMapping: elementMapping),
                "longtext",
                dbType: System.Data.DbType.String))
        {
        }

        private GuidArrayTypeMapping(RelationalTypeMappingParameters parameters)
            : base(parameters)
        {
        }

        protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters)
        {
            return new GuidArrayTypeMapping(parameters);
        }
    }
}
