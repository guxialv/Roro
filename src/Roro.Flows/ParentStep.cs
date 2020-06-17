using System.Linq;
using System.Text.Json;

namespace Roro.Flows
{
    public abstract class ParentStep : Step
    {
        protected ParentStep(Flow parent) : base(parent)
        {
            Inputs = new StepInputCollection(this);
            Outputs = new StepOutputCollection(this);
            Steps = new StepCollection(this);
        }

        protected ParentStep(Flow parent, JsonElement jsonElement) : base(parent)
        {
            Inputs = new StepInputCollection(this, jsonElement.GetProperty(nameof(Inputs)));
            Outputs = new StepOutputCollection(this, jsonElement.GetProperty(nameof(Outputs)));
            Steps = new StepCollection(this, jsonElement.GetProperty(nameof(Steps)));
        }

        internal override void ToJson(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteString(nameof(Type), Type);
            writer.WritePropertyName(nameof(Inputs));
            Inputs.ToJson(writer);
            writer.WritePropertyName(nameof(Outputs));
            Outputs.ToJson(writer);
            writer.WritePropertyName(nameof(Steps));
            Steps.ToJson(writer);
            writer.WriteEndObject();
        }

        public StepInputCollection Inputs { get; }

        public StepOutputCollection Outputs { get; }

        public StepCollection Steps { get; }

        protected Step? GetFirstStep()
        {
            return Steps.FirstOrDefault();
        }
    }
}
