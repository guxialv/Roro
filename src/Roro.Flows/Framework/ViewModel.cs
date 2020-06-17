using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Roro.Flows.Framework
{
    public abstract class ViewModel
    {
        public byte[] ToBytes()
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });
            ToJson(writer);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            return stream.ToArray();
        }

        public string ToJson()
        {
            return Encoding.UTF8.GetString(ToBytes());
        }

        internal virtual void ToJson(Utf8JsonWriter writer)
        {
            JsonSerializer.Serialize(writer, this);
        }
    }

    public abstract class ViewModel<TParent> : ViewModel
        where TParent : ViewModel
    {
        internal TParent Parent { get; }

        protected ViewModel(TParent parent)
        {
            Parent = parent;   
        }
    }

    public abstract class ViewModel<TParent, TCollection, TItem> : ViewModel<TParent>
        where TParent : ViewModel
        where TCollection : ViewModelCollection<TParent, TCollection, TItem>
        where TItem : ViewModel<TParent, TCollection, TItem>
    {
        private TCollection? _parentCollection;

        internal TCollection? ParentCollection
        {
            get => _parentCollection;
            set
            {
                if (value == _parentCollection)
                    return;
                if (value is null && _parentCollection!.Contains((TItem)this))
                    throw new InvalidOperationException("The item is not removed in the collection");
                if (value != null && !value.Contains((TItem)this))
                    throw new InvalidOperationException("The item is not added in the collection");
                _parentCollection = value;
            }
        }

        protected ViewModel(TParent parent) : base(parent)
        {
        }
    }
}
