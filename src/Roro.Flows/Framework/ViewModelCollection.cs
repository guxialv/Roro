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

        public virtual void ToJson(Utf8JsonWriter writer)
        {
            writer.WriteStartArray();
            Items.ToList().ForEach(item => item.ToJson(writer));
            writer.WriteEndArray();
        }
    }
}
