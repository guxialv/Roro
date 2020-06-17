using Roro.Flows.Framework;
using System.Text.Json;

namespace Roro.Flows
{
    public sealed class StepInputCollection : NameTypeValueCollection<Step, StepInputCollection, StepInput>
    {
        internal StepInputCollection(Step parent) : base(parent)
        {
        }

        internal StepInputCollection(Step parent, JsonElement jsonElement) : base(parent, jsonElement)
        {
        }

        protected override StepInput CreateItem(JsonElement jsonElement)
        {
            return new StepInput(Parent, jsonElement);
        }
    }
}
