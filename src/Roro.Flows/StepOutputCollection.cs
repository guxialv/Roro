using Roro.Flows.Framework;
using System.Text.Json;

namespace Roro.Flows
{
    public sealed class StepOutputCollection : NameTypeValueCollection<Step, StepOutputCollection, StepOutput>
    {
        internal StepOutputCollection(Step parent) : base(parent)
        {
        }

        internal StepOutputCollection(Step parent, JsonElement jsonElement) : base(parent, jsonElement)
        {
        }

        protected override StepOutput CreateItem(JsonElement jsonElement)
        {
            return new StepOutput(Parent, jsonElement);
        }
    }
}
