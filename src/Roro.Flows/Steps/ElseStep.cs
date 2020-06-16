using Roro.Flows.Execution;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roro.Flows.Steps
{
    public sealed class ElseStep : ParentStep
    {
        internal ElseStep(StepCollection parentStepCollection) : base(parentStepCollection)
        {
        }

        internal ElseStep(StepCollection parentStepCollection, JsonElement jsonElement) : base(parentStepCollection, jsonElement)
        {
        }

        protected override async Task<ExecutionResult> ExecuteAsync(ExecutionContext context)
        {
            await Task.CompletedTask;
            if (context.IsFirstEntry)
            {
                if (GetFirstStep() is Step firstStep)
                {
                    context.PushCall(new CallStackFrame(firstStep));
                }
                else
                {
                    context.PopCall();
                    if (GetNextStep() is Step nextStep)
                    {
                        context.PushCall(new CallStackFrame(nextStep));
                    }
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
