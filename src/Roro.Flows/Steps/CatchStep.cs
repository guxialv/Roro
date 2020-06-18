using Roro.Flows.Execution;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roro.Flows.Steps
{
    public sealed class CatchStep : Step
    {
        internal CatchStep(Flow parent) : base(parent)
        {
            Steps = new StepCollection(parent, this);
        }

        internal CatchStep(Flow parent, JsonElement jsonElement) : base(parent, jsonElement)
        {
        }

        protected override async Task<ExecutionResult> ExecuteAsync(ExecutionContext context)
        {
            await Task.CompletedTask;
            if (context.IsFirstEntry)
            {
                if (Steps!.FirstOrDefault() is Step nextStep)
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
                if (ParentCollection!.NextOrDefault(this) is Step nextStep)
                {
                    context.PushCall(new CallStackFrame(nextStep));
                }
            }
            return ExecutionResult.Completed;
        }
    }
}
