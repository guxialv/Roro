using Roro.Flows.Framework;
using System.Text.Json;

namespace Roro.Flows
{
    public sealed class FlowInput : NameTypeValue<Flow, FlowInputCollection, FlowInput>
    {
        internal FlowInput(Flow parent) : base(parent)
        {
        }

        internal FlowInput(Flow parent, JsonElement jsonElement) : base(parent, jsonElement)
        {
        }

        public override bool NameIsReadOnly => false;

        public override bool TypeIsReadOnly => false;

        public override bool ValueIsReadOnly => false;
    }
}
