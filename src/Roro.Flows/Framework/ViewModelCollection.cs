using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Roro.Flows.Framework
{
    public abstract class ViewModelCollection<TItem> : ObservableCollection<TItem>
        where TItem : ViewModel
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

        public void ToJson(Utf8JsonWriter writer)
        {
            writer.WriteStartArray();
            Items.ToList().ForEach(item => item.ToJson(writer));
            writer.WriteEndArray();
        }
    }

    public abstract class ViewModelCollection<TParent, TCollection, TItem> : ViewModelCollection<TItem>
        where TParent : ViewModel
        where TCollection : ViewModelCollection<TParent, TCollection, TItem>
        where TItem : ViewModel<TParent, TCollection, TItem>
    {
        internal TParent Parent { get; }

        protected ViewModelCollection(TParent parent)
        {
            Parent = parent;
        }

        protected ViewModelCollection(TParent parent, JsonElement jsonElement) : this(parent)
        {
            jsonElement.EnumerateArray().ToList().ForEach(jsonItem =>
            {
                var item = CreateItem(jsonItem);
                Items.Add(item);
                item.ParentCollection = (TCollection)this;
            });
        }

        protected abstract TItem CreateItem(JsonElement jsonElement);

        protected override void ClearItems()
        {
            var items = Items.ToList();
            base.ClearItems();
            items.ForEach(item => item.ParentCollection = null);
        }

        protected override void InsertItem(int index, TItem item)
        {
            if (item.Parent != Parent)
                throw new ArgumentException("The item is in another parent");
            if (item.ParentCollection != null)
                throw new ArgumentException("The item is in another collection");
            base.InsertItem(index, item);
            item.ParentCollection = (TCollection)this;
        }

        protected override void RemoveItem(int index)
        {
            var item = Items[index];
            base.RemoveItem(index);
            item.ParentCollection = null;
        }

        protected override void SetItem(int index, TItem item)
        {
            if (item.Parent != Parent)
                throw new ArgumentException("The item is in another parent");
            if (item.ParentCollection != null)
                throw new ArgumentException("The item is in another collection");
            var oldItem = Items[index];
            base.SetItem(index, item);
            oldItem.ParentCollection = null;
            item.ParentCollection = (TCollection)this;
        }
    }
}
