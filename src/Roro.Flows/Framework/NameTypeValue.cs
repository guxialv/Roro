using System.Text.Json;

namespace Roro.Flows.Framework
{
    public abstract class NameTypeValue<TParent, TCollection, TItem>
        : ViewModel<TParent, TCollection, TItem>
        where TParent : ViewModel
        where TCollection : NameTypeValueCollection<TParent, TCollection, TItem>
        where TItem : NameTypeValue<TParent, TCollection, TItem>
    {
        private string _name;
        private string _type;
        private string _value;

        protected NameTypeValue(TParent parent) : base(parent)
        {
            _name = string.Empty;
            _type = typeof(string).Name;
            _value = string.Empty;
        }

        protected NameTypeValue(TParent parent, JsonElement jsonElement) : base(parent)
        {
            _name = jsonElement.GetProperty(nameof(Name)).GetString();
            _type = jsonElement.GetProperty(nameof(Type)).GetString();
            _value = jsonElement.GetProperty(nameof(Value)).GetString();
        }

        internal override void ToJson(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteString(nameof(Name), Name);
            writer.WriteString(nameof(Type), Type);
            writer.WriteString(nameof(Value), Value);
            writer.WriteEndObject();
        }

        public virtual string Name
        {
            get => _name;
            set
            {
                ViewModelReadOnlyException.ThowIfReadOnly(NameIsReadOnly);
                _name = value;
            }
        }

        public virtual string Type
        {
            get => _type;
            set
            {
                ViewModelReadOnlyException.ThowIfReadOnly(TypeIsReadOnly);
                _type = value;
            }
        }

        public virtual string Value
        {
            get => _value;
            set
            {
                ViewModelReadOnlyException.ThowIfReadOnly(NameIsReadOnly);
                _value = value;
            }
        }

        public abstract bool NameIsReadOnly { get; }

        public abstract bool TypeIsReadOnly { get; }

        public abstract bool ValueIsReadOnly { get; }
    }
}
