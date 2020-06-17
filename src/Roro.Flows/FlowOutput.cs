using Roro.Flows.Framework;
using System.Text.Json;

namespace Roro.Flows
{
    public sealed class FlowOutput : NameTypeValue<Flow, FlowOutputCollection, FlowOutput>
    {
        internal FlowOutput(Flow parent) : base(parent)
        {
        }

        internal FlowOutput(Flow parent, JsonElement jsonElement) : base(parent, jsonElement)
        {
        }

        public override bool NameIsReadOnly => false;

        public override bool TypeIsReadOnly => false;

        public override bool ValueIsReadOnly => false;
    }
}
