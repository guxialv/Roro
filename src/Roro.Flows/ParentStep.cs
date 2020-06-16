using System.Linq;
using System.Text.Json;

namespace Roro.Flows
{
    public abstract class ParentStep : Step
    {
        protected ParentStep(StepCollection parentStepCollection) : base(parentStepCollection)
        {
            Inputs = new StepInputCollection(this);
            Outputs = new StepOutputCollection(this);
            Steps = new StepCollection(this);
        }

        protected ParentStep(StepCollection parentStepCollection, JsonElement jsonElement) : base(parentStepCollection)
        {
            Steps = new StepCollection(this, jsonElement.GetProperty(nameof(Steps)));
        }

        public override void ToJson(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteString(nameof(Type), Type);
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
