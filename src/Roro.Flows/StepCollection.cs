using Roro.Flows.Framework;
using System.Text.Json;

namespace Roro.Flows
{
    public sealed class StepCollection : ViewModelCollection<Flow, StepCollection, Step>
    {
        internal ParentStep? ParentStep { get; }

        internal StepCollection(Flow parent) : base(parent)
        {
        }

        internal StepCollection(Flow parent, JsonElement jsonElement) : base(parent, jsonElement)
        {
        }

        internal StepCollection(ParentStep parentStep) : base(parentStep.Parent)
        {
            ParentStep = parentStep;
        }

        internal StepCollection(ParentStep parentStep, JsonElement jsonElement) : base(parentStep.Parent, jsonElement)
        {
            ParentStep = parentStep;
        }

        protected override Step CreateItem(JsonElement jsonElement)
        {
            return Step.Create(Parent, jsonElement);
        }

        protected override void InsertItem(int index, Step item)
        {
            base.InsertItem(index, item);
        }

        public TStep AddNew<TStep>() where TStep : Step
        {
            var step = Step.Create<TStep>(Parent);
            Add(step);
            return (TStep)step;
        }
    }
}