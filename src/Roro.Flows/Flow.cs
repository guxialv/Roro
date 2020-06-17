using Roro.Flows.Execution;
using Roro.Flows.Framework;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roro.Flows
{
    public sealed class Flow : ViewModel<FlowApp, FlowCollection, Flow>, IExecutable
    {
        public string Id => Path;

        internal Flow(FlowApp parent, string path) : base(parent)
        {
            Path = path;
            Inputs = new FlowInputCollection(this);
            Outputs = new FlowOutputCollection(this);
            Variables = new FlowVariableCollection(this);
            Steps = new StepCollection(this);
        }

        internal Flow(FlowApp parent, string path, JsonElement jsonElement) : base(parent)
        {
            Path = path;
            Inputs = new FlowInputCollection(this, jsonElement.GetProperty(nameof(Inputs)));
            Outputs = new FlowOutputCollection(this, jsonElement.GetProperty(nameof(Outputs)));
            Variables = new FlowVariableCollection(this, jsonElement.GetProperty(nameof(Variables)));
            Steps = new StepCollection(this, jsonElement.GetProperty(nameof(Steps)));
        }

        internal override void ToJson(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(nameof(Inputs));
            Inputs.ToJson(writer);
            writer.WritePropertyName(nameof(Outputs));
            Outputs.ToJson(writer);
            writer.WritePropertyName(nameof(Variables));
            Variables.ToJson(writer);
            writer.WritePropertyName(nameof(Steps));
            Steps.ToJson(writer);
            writer.WriteEndObject();
        }

        public string Path { get; }

        public FlowInputCollection Inputs { get; }

        public FlowOutputCollection Outputs { get; }

        public FlowVariableCollection Variables { get; }

        public StepCollection Steps { get; }

        async Task<ExecutionResult> IExecutable.ExecuteAsync(ExecutionContext context)
        {
            await Task.CompletedTask;
            if (context.IsFirstEntry)
            {
                context.Inputs.Clear(); // set inputs
                if (Steps.FirstOrDefault() is Step firstStep)
                {
                    context.PushCall(new CallStackFrame(firstStep));
                    return ExecutionResult.Running;
                }
                else
                {
                    throw new Exception("Cannot execute the flow without steps");
                }
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
