using System.Collections.ObjectModel;

namespace DataType
{
    public sealed class RowCollection : Collection<Row>
    {
        internal readonly Table _table;

        public RowCollection(Table table)
        {
            _table = table;
        }
    }
}
