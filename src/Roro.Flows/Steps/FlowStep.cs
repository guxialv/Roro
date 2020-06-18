using Roro.Flows.Execution;
using Roro.Flows.Services;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roro.Flows.Steps
{
    public sealed class FlowStep : Step
    {
        internal FlowStep(Flow parent) : base(parent)
        {
            SubType = string.Empty;
            Inputs = new StepInputCollection(this);
            Outputs = new StepOutputCollection(this);
        }

        internal FlowStep(Flow parent, JsonElement jsonElement) : base(parent, jsonElement)
        {
        }

        protected override async Task<ExecutionResult> ExecuteAsync(ExecutionContext context)
        {
            if (context.IsFirstEntry)
            {
                Flow? flow;
                try
                {
                    var flowPath = SubType!;
                    var jsonText = await Parent.Parent.Services.GetShared<IFlowPickerService>()!.GetFileContentsAsync(flowPath);
                    flow = new Flow(Parent.Parent, flowPath, JsonDocument.Parse(jsonText).RootElement);
                }
                catch (Exception exception)
                {
                    await ThrowAsync(context, exception);
                    return ExecutionResult.Failed;
                }
                context.Inputs.Clear(); // set inputs
                context.Locals.Add(nameof(flow), flow);
                context.PushCall(new CallStackFrame(flow));
                return ExecutionResult.Running;
            }
            else
            {
                context.Outputs.Clear(); // get outputs
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
