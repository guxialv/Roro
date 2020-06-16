using Roro.Flows.Execution;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roro.Flows.Steps
{
    public sealed class CatchStep : ParentStep
    {
        internal CatchStep(StepCollection parentStepCollection) : base(parentStepCollection)
        {
        }

        internal CatchStep(StepCollection parentStepCollection, JsonElement jsonElement) : base(parentStepCollection, jsonElement)
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
                if (GetNextStep() is Step nextStep)
                {
                    context.PushCall(new CallStackFrame(nextStep));
                }
            }
            return ExecutionResult.Completed;
        }
    }
}
