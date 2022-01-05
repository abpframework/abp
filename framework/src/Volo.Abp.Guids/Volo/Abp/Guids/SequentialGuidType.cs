namespace Volo.Abp.Guids;

/// <summary>
/// Describes the type of a sequential GUID value.
/// </summary>
public enum SequentialGuidType
{
    /// <summary>
    /// The GUID should be sequential when formatted using the <see cref="Guid.ToString()" /> method.
    /// Used by MySql and PostgreSql.
    /// </summary>
    SequentialAsString,

    /// <summary>
    /// The GUID should be sequential when formatted using the <see cref="Guid.ToByteArray" /> method.
    /// Used by Oracle.
    /// </summary>
    SequentialAsBinary,

    /// <summary>
    /// The sequential portion of the GUID should be located at the end of the Data4 block.
    /// Used by SqlServer.
    /// </summary>
    SequentialAtEnd
}
