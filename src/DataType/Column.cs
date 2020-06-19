using System;

namespace DataType
{
    public sealed class Column
    {
        internal readonly ColumnCollection _columns;

        internal Column(ColumnCollection columns)
        {
            _columns = columns;
            Name = string.Empty;
            Type = typeof(string);
        }

        public string Name { get; set; }

        public Type Type { get; set; }
    }
}
