using Roro.Flows.Execution;
using Roro.Flows.Framework;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roro.Flows
{
    public sealed class Flow : ViewModel, IExecutable
    {
        internal readonly FlowCollection _parentFlowCollection;

        internal Flow(FlowCollection parentFlowCollection, string path)
        {
            _parentFlowCollection = parentFlowCollection;
            Path = path;
            Steps = new StepCollection(this);
        }

        internal Flow(FlowCollection parentFlowCollection, string path, JsonElement jsonElement) : this(parentFlowCollection, path)
        {
            Steps = new StepCollection(this, jsonElement.GetProperty(nameof(Steps)));
        }

        public override void ToJson(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(nameof(Steps));
            Steps.ToJson(writer);
            writer.WriteEndObject();
        }

        public string Path { get; }

        public StepCollection Steps { get; }

        async Task<ExecutionResult> IExecutable.ExecuteAsync(ExecutionContext context)
        {
            await Task.CompletedTask;
            if (context.IsFirstEntry)
            {
                context.Inputs.Clear(); // set inputs
                if (Steps.FirstOrDefault() is Step step)
                {
                    context.PushCall(new CallStackFrame(step));
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
