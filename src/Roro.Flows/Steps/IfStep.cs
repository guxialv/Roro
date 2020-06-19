using Roro.Flows.Execution;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roro.Flows.Steps
{
    public sealed class IfStep : Step
    {
        internal IfStep(Flow parent) : base(parent)
        {
            Call = string.Empty;
            Inputs = new StepInputCollection(this);
            Outputs = new StepOutputCollection(this);
            Steps = new StepCollection(Parent, this);
        }

        internal IfStep(Flow parent, JsonElement jsonElement) : base(parent, jsonElement)
        {
        }

        private async Task<bool> EvaluateAsync(ExecutionContext context)
        {
            return await Task.FromResult(true);
        }

        protected override async Task<ExecutionResult> ExecuteAsync(ExecutionContext context)
        {
            if (context.IsFirstEntry)
            {
                context.Inputs.Clear(); // set inputs
                bool evaluateResult;
                try
                {
                    evaluateResult = await EvaluateAsync(context);
                }
                catch (Exception exception)
                {
                    await ThrowAsync(context, exception);
                    return ExecutionResult.Failed;
                }
                context.Outputs.Clear(); // set outputs
                if (evaluateResult is true)
                {
                    if (Steps!.FirstOrDefault() is Step firstStep)
                    {
                        context.PushCall(new CallStackFrame(firstStep));
                    }
                    else
                    {
                        context.PopCall();
                        if (ParentCollection!.NextOrDefault(this, step => !(step is ElseIfStep || step is ElseStep)) is Step nextStep)
                        {
                            context.PushCall(new CallStackFrame(nextStep));
                        }
                    }
                    return ExecutionResult.EvaluatedToTrue;
                }
                else
                {
                    context.PopCall();
                    if (ParentCollection!.NextOrDefault(this, step => !(step is ElseIfStep || step is ElseStep)) is Step nextStep)
                    {
                        context.PushCall(new CallStackFrame(nextStep));
                    }
                    return ExecutionResult.EvaluatedToFalse;
                }
            }
            else
            {
                context.PopCall();
                if (ParentCollection!.NextOrDefault(this, step => !(step is ElseIfStep || step is ElseStep)) is Step nextStep)
                {
                    context.PushCall(new CallStackFrame(nextStep));
                }
                return ExecutionResult.Completed;
            }
        }
    }
}
