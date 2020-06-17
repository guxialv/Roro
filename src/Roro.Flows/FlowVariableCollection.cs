using Roro.Flows.Framework;
using System.Text.Json;

namespace Roro.Flows
{
    public sealed class FlowVariableCollection : NameTypeValueCollection<Flow, FlowVariableCollection, FlowVariable>
    {
        internal FlowVariableCollection(Flow parent) : base(parent)
        {
        }

        internal FlowVariableCollection(Flow parent, JsonElement jsonElement) : base(parent, jsonElement)
        {
        }

        protected override FlowVariable CreateItem(JsonElement jsonElement)
        {
            return new FlowVariable(Parent, jsonElement);
        }
    }
}
