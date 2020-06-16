using Roro.Flows.Execution;
using Roro.Flows.Services;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roro.Flows.Steps
{
    public sealed class CallStep : Step
    {
        public string FlowPath { get; set; } = string.Empty;

        internal CallStep(StepCollection parentStepCollection) : base(parentStepCollection)
        {
        }

        internal CallStep(StepCollection parentStepCollection, JsonElement jsonElement) : base(parentStepCollection)
        {
        }

        protected override async Task<ExecutionResult> ExecuteAsync(ExecutionContext context)
        {
            if (context.IsFirstEntry)
            {
                Flow? flow;
                try
                {
                    var path = FlowPath;
                    var json = await _parentStepCollection._parentFlow._parentFlowCollection._app._services.GetShared<IFlowPickerService>()!.GetFileContentsAsync(path);
                    flow = new Flow(_parentStepCollection._parentFlow._parentFlowCollection, path, JsonDocument.Parse(json).RootElement);
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
                return ExecutionResult.Completed;
            }
        }
    }
}
