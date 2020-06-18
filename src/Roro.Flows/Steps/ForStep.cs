using Roro.Flows.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roro.Flows.Steps
{
    public sealed class ForStep : Step
    {
        internal ForStep(Flow parent) : base(parent)
        {
            SubType = string.Empty;
            Inputs = new StepInputCollection(this);
            Outputs = new StepOutputCollection(this);
            Steps = new StepCollection(Parent, this);
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
                if (Steps!.FirstOrDefault() is Step firstStep)
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
    }
}
