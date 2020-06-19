using Roro.Flows.Execution;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roro.Flows.Steps
{
    public sealed class ElseStep : Step
    {
        internal ElseStep(Flow parent) : base(parent)
        {
            Steps = new StepCollection(Parent, this);
        }

        internal ElseStep(Flow parent, JsonElement jsonElement) : base(parent, jsonElement)
        {
        }

        protected override async Task<ExecutionResult> ExecuteAsync(ExecutionContext context)
        {
            await Task.CompletedTask;
            if (context.IsFirstEntry)
            {
                if (!(ParentCollection!.PreviousOrDefault(this) is Step previousStep && (previousStep is IfStep || previousStep is ElseIfStep)))
                {
                    throw new Exception("The ElseStep must be after an IfStep or an ElseIfStep");
                }
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
            }
            else
            {
                context.PopCall();
                if (ParentCollection!.NextOrDefault(this) is Step nextStep)
                {
                    context.PushCall(new CallStackFrame(nextStep));
                }
            }
            return ExecutionResult.Completed;
        }
    }
}
