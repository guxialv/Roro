using System.Text.Json;

namespace Roro.Flows.Framework
{
    public abstract class NameTypeValue<TParent, TCollection, TItem>
        : ViewModel<TParent, TCollection, TItem>
        where TParent : ViewModel
        where TCollection : NameTypeValueCollection<TParent, TCollection, TItem>
        where TItem : NameTypeValue<TParent, TCollection, TItem>
    {
        protected NameTypeValue(TParent parent) : base(parent)
        {
            Name = string.Empty;
            Type = typeof(string).Name;
            Value = string.Empty;
        }

        protected NameTypeValue(TParent parent, JsonElement jsonElement) : base(parent)
        {
            Name = jsonElement.GetProperty(nameof(Name)).GetString();
            Type = jsonElement.GetProperty(nameof(Type)).GetString();
            Value = jsonElement.GetProperty(nameof(Value)).GetString();
        }

        public virtual string Name { get; set; }

        public virtual string Type { get; set; }

        public virtual string Value { get; set; }

        public abstract bool NameIsReadOnly { get; }

        public abstract bool TypeIsReadOnly { get; }

        public abstract bool ValueIsReadOnly { get; }
    }
}
