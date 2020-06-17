using Roro.Flows.Execution;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roro.Flows.Steps
{
    public sealed class CatchStep : ParentStep
    {
        internal CatchStep(Flow parent) : base(parent)
        {
        }

        internal CatchStep(Flow parent, JsonElement jsonElement) : base(parent, jsonElement)
        {
        }

        protected override async Task<ExecutionResult> ExecuteAsync(ExecutionContext context)
        {
            await Task.CompletedTask;
            if (context.IsFirstEntry)
            {
                if (GetFirstStep() is Step nextStep)
                {
                    context.PushCall(new CallStackFrame(nextStep));
                }
                else
                {
                    context.PopCall();
                }
            }
            else
            {
                context.PopCall();
                if (NextOrDefault() is Step nextStep)
                {
                    context.PushCall(new CallStackFrame(nextStep));
                }
            }
            return ExecutionResult.Completed;
        }
    }
}
