using System.Collections.ObjectModel;

namespace DataType
{
    public sealed class ColumnCollection : Collection<Column>
    {
        internal readonly Table _table;

        internal ColumnCollection(Table table)
        {
            _table = table;
        }
    }
}
