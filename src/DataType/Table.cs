namespace DataType
{
    public sealed class Table
    {
        public Table()
        {
            Columns = new ColumnCollection(this);
            Rows = new RowCollection(this);
        }

        public ColumnCollection Columns { get; }

        public RowCollection Rows { get; }
    }
}
