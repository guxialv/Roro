using Roro.Flows.Execution;
using Roro.Flows.Services;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roro.Flows.Steps
{
    public sealed class CallStep : Step
    {
        internal CallStep(Flow parent) : base(parent)
        {
            FlowPath = string.Empty;
            Inputs = new StepInputCollection(this);
            Outputs = new StepOutputCollection(this);
        }

        internal CallStep(Flow parent, JsonElement jsonElement) : base(parent, jsonElement)
        {
            FlowPath = jsonElement.GetProperty(nameof(FlowPath)).GetString();
            Inputs = new StepInputCollection(this, jsonElement.GetProperty(nameof(Inputs)));
            Outputs = new StepOutputCollection(this, jsonElement.GetProperty(nameof(Outputs)));
        }

        internal override void ToJson(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteString(nameof(Type), Type);
            writer.WriteString(nameof(FlowPath), FlowPath);
            writer.WritePropertyName(nameof(Inputs));
            Inputs.ToJson(writer);
            writer.WritePropertyName(nameof(Outputs));
            Outputs.ToJson(writer);
            writer.WriteEndObject();
        }
        public string FlowPath { get; set; }

        public StepInputCollection Inputs { get; }

        public StepOutputCollection Outputs { get; }

        protected override async Task<ExecutionResult> ExecuteAsync(ExecutionContext context)
        {
            if (context.IsFirstEntry)
            {
                Flow? flow;
                try
                {
                    var path = FlowPath;
                    var json = await Parent.Parent.Services.GetShared<IFlowPickerService>()!.GetFileContentsAsync(path);
                    flow = new Flow(Parent.Parent, path, JsonDocument.Parse(json).RootElement);
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
                if (NextOrDefault() is Step nextStep)
                {
                    context.PushCall(new CallStackFrame(nextStep));
                }
                return ExecutionResult.Completed;
            }
        }
    }
}
