using Roro.Flows.Framework;
using System.Text.Json;

namespace Roro.Flows
{
    public sealed class StepOutput : NameTypeValue<Step, StepOutputCollection, StepOutput>
    {
        internal StepOutput(Step parent) : base(parent)
        {
        }

        internal StepOutput(Step parent, JsonElement jsonElement) : base(parent, jsonElement)
        {
        }

        public override bool NameIsReadOnly => false;

        public override bool TypeIsReadOnly => false;

        public override bool ValueIsReadOnly => false;
    }
}
