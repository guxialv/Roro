using Roro.Flows.Execution;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roro.Flows.Steps
{
    public sealed class ThrowStep : Step
    {
        internal ThrowStep(Flow parent) : base(parent)
        {
        }

        internal ThrowStep(Flow parent, JsonElement jsonElement) : base(parent, jsonElement)
        {
        }

        protected override async Task<ExecutionResult> ExecuteAsync(ExecutionContext context)
        {
            context.Inputs.Clear(); // set inputs
            context.Outputs.Clear(); // set outputs
            await ThrowAsync(context, new Exception("HANDLED_ERROR"));
            return ExecutionResult.Completed;
        }
    }
}
