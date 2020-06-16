using Roro.Flows.Execution;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roro.Flows.Steps
{
    public sealed class TryStep : ParentStep
    {
        internal TryStep(StepCollection parentStepCollection) : base(parentStepCollection)
        {
        }

        internal TryStep(StepCollection parentStepCollection, JsonElement jsonElement) : base(parentStepCollection, jsonElement)
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
                if (GetNextStep(step => !(step is CatchStep)) is Step nextStep)
                {
                    context.PushCall(new CallStackFrame(nextStep));
                }
            }
            return ExecutionResult.Completed;
        }
    }
}
