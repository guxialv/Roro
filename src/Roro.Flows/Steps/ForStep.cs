using Roro.Flows.Execution;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roro.Flows.Steps
{
    public sealed class ForStep : ParentStep
    {
        internal ForStep(Flow parent) : base(parent)
        {
        }

        internal ForStep(Flow parent, JsonElement jsonElement) : base(parent, jsonElement)
        {
        }

        private async Task<bool> EvaluateAsync(ExecutionContext context)
        {
            if (context.IsFirstEntry)
            {
                var collection = new List<object>();
                context.Locals.Add(string.Empty, collection);
            }
            else
            {
                var collection = context.Locals[string.Empty];
            }
            return await Task.FromResult(true);
        }

        protected override async Task<ExecutionResult> ExecuteAsync(ExecutionContext context)
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
                    if (NextOrDefault() is Step nextStep)
                    {
                        context.PushCall(new CallStackFrame(nextStep));
                    }
                }
                return ExecutionResult.EvaluatedToTrue;
            }
            else
            {
                context.PopCall();
                if (NextOrDefault() is Step nextStep)
                {
                    context.PushCall(new CallStackFrame(nextStep));
                }
                return ExecutionResult.EvaluatedToFalse;
            }
        }
    }
}
