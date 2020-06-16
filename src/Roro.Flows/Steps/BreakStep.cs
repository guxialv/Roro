using Roro.Flows.Execution;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roro.Flows.Steps
{
    public sealed class BreakStep : Step
    {
        internal BreakStep(StepCollection parentStepCollection) : base(parentStepCollection)
        {
        }

        internal BreakStep(StepCollection parentStepCollection, JsonElement jsonElement) : base(parentStepCollection)
        {
        }

        protected override async Task<ExecutionResult> ExecuteAsync(ExecutionContext context)
        {
            await Task.CompletedTask;
            context.PopCall();
            while (true)
            {
                var call = context.PeekCall();
                if (call is null || call.Executable is Flow)
                {
                    throw new Exception("Cannot find a loop to break");
                }
                if (call.Executable is ForStep || call.Executable is WhileStep)
                {
                    call.AllowReentry = false; // break
                    return ExecutionResult.Completed;
                }
                else
                {
                    context.PopCall();
                }
            }
        }
    }
}
