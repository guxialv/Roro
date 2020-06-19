using System.Collections.Generic;

namespace DataType
{
    public sealed class Row : Dictionary<Column, Cell>
    {
        internal readonly RowCollection _rows;

        public Row(RowCollection rows)
        {
            _rows = rows;
        }
    }
}
