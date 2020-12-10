using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Components.Extensibility.TableColumns
{
    public class TableColumnsConfiguration
    {
        protected TableColumnDictionary TableColumns { get; set; }

        public TableColumnsConfiguration()
        {
            TableColumns = new TableColumnDictionary();
        }

        public List<TableColumn> Get<T>()
        {
            return TableColumns.GetOrAdd(typeof(T).FullName, () => new List<TableColumn>());
        }
    }
}
