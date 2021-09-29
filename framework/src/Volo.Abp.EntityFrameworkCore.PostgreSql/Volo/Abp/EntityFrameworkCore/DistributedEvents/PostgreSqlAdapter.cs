namespace Volo.Abp.EntityFrameworkCore.DistributedEvents
{
    public class PostgreSqlAdapter : ISqlAdapter
    {
        public string NormalizeTableName(string tableName)
        {
            return $"\"{tableName}\"";
        }

        public string NormalizeColumnName(string columnName)
        {
            return $"\"{columnName}\"";
        }

        public string NormalizeColumnNameEqualsValue(string columnName, object value)
        {
            return $"\"{columnName}\" = '{value}'";
        }

        public string NormalizeValue(object value)
        {
            return $"'{value}'";
        }
    }
}
