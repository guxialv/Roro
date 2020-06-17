using Roro.Flows.Framework;
using System.Text.Json;

namespace Roro.Flows
{
    public sealed class FlowOutputCollection : NameTypeValueCollection<Flow, FlowOutputCollection, FlowOutput>
    {
        internal FlowOutputCollection(Flow parent) : base(parent)
        {
        }

        internal FlowOutputCollection(Flow parent, JsonElement jsonElement) : base(parent, jsonElement)
        {
        }

        protected override FlowOutput CreateItem(JsonElement jsonElement)
        {
            return new FlowOutput(Parent, jsonElement);
        }
    }
}
