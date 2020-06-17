using Roro.Flows.Execution;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roro.Flows.Steps
{
    public sealed class WhileStep : ParentStep
    {
        internal WhileStep(Flow parent) : base(parent)
        {
        }

        internal WhileStep(Flow parent, JsonElement jsonElement) : base(parent, jsonElement)
        {
        }

        private async Task<bool> EvaluateAsync(ExecutionContext context)
        {
            return await Task.FromResult(true);
        }

        protected override async Task<ExecutionResult> ExecuteAsync(ExecutionContext context)
        {
            if (context.AllowReentry)
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
                        if (ParentCollection!.NextOrDefault(this) is Step nextStep)
                        {
                            context.PushCall(new CallStackFrame(nextStep));
                        }
                    }
                    return ExecutionResult.EvaluatedToTrue;
                }
                else
                {
                    context.PopCall();
                    if (ParentCollection!.NextOrDefault(this) is Step nextStep)
                    {
                        context.PushCall(new CallStackFrame(nextStep));
                    }
                    return ExecutionResult.EvaluatedToFalse;
                }
            }
            else
            {
                context.PopCall();
                if (ParentCollection!.NextOrDefault(this) is Step nextStep)
                {
                    context.PushCall(new CallStackFrame(nextStep));
                }
                return ExecutionResult.Completed;
            }
        }
    }
}
