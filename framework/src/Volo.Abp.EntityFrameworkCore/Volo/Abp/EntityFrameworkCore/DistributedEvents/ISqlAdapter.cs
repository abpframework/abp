namespace Volo.Abp.EntityFrameworkCore.DistributedEvents
{
    public interface ISqlAdapter
    {
        string NormalizeTableName(string tableName);

        string NormalizeColumnName(string columnName);

        string NormalizeColumnNameEqualsValue(string columnName, object value);

        string NormalizeValue(object value);
    }
}
