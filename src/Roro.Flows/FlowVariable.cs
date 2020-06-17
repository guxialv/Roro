using Roro.Flows.Framework;
using System.Text.Json;

namespace Roro.Flows
{
    public sealed class FlowVariable : NameTypeValue<Flow, FlowVariableCollection, FlowVariable>
    {
        internal FlowVariable(Flow parent) : base(parent)
        {
        }

        internal FlowVariable(Flow parent, JsonElement jsonElement) : base(parent, jsonElement)
        {
        }

        public override bool NameIsReadOnly => false;

        public override bool TypeIsReadOnly => false;

        public override bool ValueIsReadOnly => false;
    }
}
