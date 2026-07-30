using System;
using System.Data;
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
            return new StringTypeMapping("longtext", DbType.String).Clone(
                clrType: typeof(Guid[]),
                elementMapping: mappingInfo.ElementTypeMapping);
        }

        return null;
    }
}
