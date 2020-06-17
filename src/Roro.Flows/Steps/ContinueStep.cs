using Roro.Flows.Execution;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roro.Flows.Steps
{
    public sealed class ContinueStep : Step
    {
        internal ContinueStep(Flow parent) : base(parent)
        {
        }

        internal ContinueStep(Flow parent, JsonElement jsonElement) : base(parent, jsonElement)
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
                    throw new Exception("The continue step cannot find any loop step");
                }
                if (call.Executable is ForStep || call.Executable is WhileStep)
                {
                    call.AllowReentry = true; // continue
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
