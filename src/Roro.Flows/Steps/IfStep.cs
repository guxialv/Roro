using Roro.Flows.Execution;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roro.Flows.Steps
{
    public sealed class IfStep : ParentStep
    {
        internal IfStep(StepCollection parentStepCollection) : base(parentStepCollection)
        {
        }

        internal IfStep(StepCollection parentStepCollection, JsonElement jsonElement) : base(parentStepCollection, jsonElement)
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
                    if (GetFirstStep() is Step firstStep)
                    {
                        context.PushCall(new CallStackFrame(firstStep));
                    }
                    else
                    {
                        context.PopCall();
                        if (GetNextStep(step => !(step is ElseIfStep || step is ElseStep)) is Step nextStep)
                        {
                            context.PushCall(new CallStackFrame(nextStep));
                        }
                    }
                    return ExecutionResult.EvaluatedToTrue;
                }
                else
                {
                    context.PopCall();
                    if (GetNextStep(step => !(step is ElseIfStep || step is ElseStep)) is Step nextStep)
                    {
                        context.PushCall(new CallStackFrame(nextStep));
                    }
                    return ExecutionResult.EvaluatedToFalse;
                }
            }
            else
            {
                context.PopCall();
                if (GetNextStep(step => !(step is ElseIfStep || step is ElseStep)) is Step nextStep)
                {
                    context.PushCall(new CallStackFrame(nextStep));
                }
                return ExecutionResult.Completed;
            }
        }
    }
}
