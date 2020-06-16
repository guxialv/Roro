using Roro.Flows.Framework;
using System.Linq;
using System.Text.Json;

namespace Roro.Flows
{
    public sealed class StepInputCollection : NameTypeValueCollection<StepInput>
    {
        internal readonly Step _parentStep;

        internal StepInputCollection(Step parentStep)
        {
            _parentStep = parentStep;
        }

        internal StepInputCollection(Step parentStep, JsonElement jsonElement) : this(parentStep)
        {
            jsonElement.EnumerateArray().ToList().ForEach(step => Add(Step.Create(this, step)));
        }
    }
}
