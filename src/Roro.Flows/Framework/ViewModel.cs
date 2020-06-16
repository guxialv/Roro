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

        public virtual void ToJson(Utf8JsonWriter writer)
        {
            JsonSerializer.Serialize(writer, this);
        }
    }
}
