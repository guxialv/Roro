using Roro.Flows.Execution;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roro.Flows.Steps
{
    public sealed class ThrowStep : Step
    {
        internal ThrowStep(StepCollection parentStepCollection) : base(parentStepCollection)
        {
        }

        internal ThrowStep(StepCollection parentStepCollection, JsonElement jsonElement) : base(parentStepCollection)
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
