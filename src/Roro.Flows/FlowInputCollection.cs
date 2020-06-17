using Roro.Flows.Framework;
using System.Text.Json;

namespace Roro.Flows
{
    public sealed class FlowInputCollection : NameTypeValueCollection<Flow, FlowInputCollection, FlowInput>
    {
        internal FlowInputCollection(Flow parent) : base(parent)
        {
        }

        internal FlowInputCollection(Flow parent, JsonElement jsonElement) : base(parent, jsonElement)
        {
        }

        protected override FlowInput CreateItem(JsonElement jsonElement)
        {
            return new FlowInput(Parent, jsonElement);
        }
    }
}
