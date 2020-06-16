using Roro.Flows.Execution;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roro.Flows.Steps
{
    public sealed class ActionStep : Step
    {
        internal ActionStep(StepCollection parentStepCollection) : base(parentStepCollection)
        {
        }

        internal ActionStep(StepCollection parentStepCollection, JsonElement jsonElement) : base(parentStepCollection)
        { 
        }

        protected override async Task<ExecutionResult> ExecuteAsync(ExecutionContext context)
        {
            context.Inputs.Clear(); // set inputs here
            try
            {
                await Task.CompletedTask; // execute action here
                context.PopCall();
                if (GetNextStep() is Step nextStep)
                {
                    context.PushCall(new CallStackFrame(nextStep));
                }
            }
            catch (Exception exception)
            {
                await ThrowAsync(context, exception);
                return ExecutionResult.Failed;
            }
            context.Outputs.Clear(); // get outputs here
            return ExecutionResult.Completed;
        }
    }
}
