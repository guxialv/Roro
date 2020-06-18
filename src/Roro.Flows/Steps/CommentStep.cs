using Roro.Flows.Execution;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roro.Flows.Steps
{
    public sealed class CommentStep : Step
    {
        internal CommentStep(Flow parent) : base(parent)
        {
            Inputs = new StepInputCollection(this);
        }

        internal CommentStep(Flow parent, JsonElement jsonElement) : base(parent, jsonElement)
        {
        }

        protected override async Task<ExecutionResult> ExecuteAsync(ExecutionContext context)
        {
            await Task.CompletedTask;
            context.PopCall();
            if (ParentCollection!.NextOrDefault(this) is Step nextStep)
            {
                context.PushCall(new CallStackFrame(nextStep));
            }
            return ExecutionResult.Completed;
        }
    }
}
