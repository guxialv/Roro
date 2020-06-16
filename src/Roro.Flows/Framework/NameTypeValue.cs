namespace Roro.Flows.Framework
{
    public abstract class NameTypeValue : ViewModel
    {
        protected NameTypeValue()
        {
            Name = string.Empty;
            Type = typeof(string).Name;
            Value = string.Empty;
        }

        public virtual string Name { get; set; }

        public virtual string Type { get; set; }

        public virtual string Value { get; set; }

        public abstract bool NameIsReadOnly { get; }

        public abstract bool TypeIsReadOnly { get; }

        public abstract bool ValueIsReadOnly { get; }
    }
}
