using System;
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
            return Get(typeof(T));
        }
        
        public List<TableColumn> Get(Type type)
        {
            return TableColumns.GetOrAdd(type.FullName, () => new List<TableColumn>());
        }
    }
}
