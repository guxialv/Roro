using System;

namespace DataType
{
    public sealed class Cell
    {
        internal readonly Row _row;
        internal readonly Column _column;

        internal Cell(Row rows, Column column)
        {
            _row = rows;
            _column = column;
        }

        public object? Value { get; set; }

        public string? GetString() => Value?.ToString();

        public int GetInt32() => int.Parse(GetString());

        public double GetDouble() => double.Parse(GetString());

        public bool GetBoolean() => bool.Parse(GetString());

        public DateTimeOffset GetDateTimeOffset() => DateTimeOffset.Parse(GetString());
    }
}
