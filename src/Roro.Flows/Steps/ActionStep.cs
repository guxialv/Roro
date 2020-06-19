using Roro.Flows.Execution;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roro.Flows.Steps
{
    public sealed class ActionStep : Step
    {
        internal ActionStep(Flow parent) : base(parent)
        {
            Call = string.Empty; 
            Inputs = new StepInputCollection(this);
            Outputs = new StepOutputCollection(this);
        }

        internal ActionStep(Flow parent, JsonElement jsonElement) : base(parent, jsonElement)
        {
        }

        protected override async Task<ExecutionResult> ExecuteAsync(ExecutionContext context)
        {
            context.Inputs.Clear(); // set inputs here
            try
            {
                await Task.CompletedTask; // execute action here
            }
            catch (Exception exception)
            {
                await ThrowAsync(context, exception);
                return ExecutionResult.Failed;
            }
            context.PopCall();
            if (ParentCollection!.NextOrDefault(this) is Step nextStep)
            {
                context.PushCall(new CallStackFrame(nextStep));
            }
            context.Outputs.Clear(); // get outputs here
            return ExecutionResult.Completed;
        }
    }
}
