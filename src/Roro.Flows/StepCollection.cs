using Roro.Flows.Framework;
using System;
using System.Linq;
using System.Text.Json;

namespace Roro.Flows
{
    public sealed class StepCollection : ViewModelCollection<Flow, StepCollection, Step>
    {
        internal Step? ParentStep { get; }

        internal StepCollection(Flow parent, Step? parentStep) : base(parent)
        {
            ParentStep = parentStep;
        }

        internal StepCollection(Flow parent, Step? parentStep, JsonElement jsonElement) : base(parent, jsonElement)
        {
            ParentStep = parentStep;
        }

        protected override Step CreateItem(JsonElement jsonElement)
        {
            return Step.Create(Parent, jsonElement);
        }

        protected override void SetItem(int index, Step item)
        {
            throw new NotImplementedException();
        }

        public TStep AddNew<TStep>() where TStep : Step
        {
            var step = Step.Create<TStep>(Parent);
            Add(step);
            return (TStep)step;
        }

        public Step? ParentOrDefault(Step step)
        {
            return ParentOrDefault(step, x => true);
        }

        public Step? ParentOrDefault(Step step, Func<Step, bool> predicate)
        {
            if (step.ParentCollection != this)
                throw new ArgumentException($"The {step} step is not in the collection");
            var parentStep = ParentStep;
            while (parentStep != null && !predicate.Invoke(parentStep))
                parentStep = parentStep.ParentStep;
            return parentStep;
        }

        public Step? PreviousOrDefault(Step step)
        {
            return PreviousOrDefault(step, x => true);
        }

        public Step? PreviousOrDefault(Step step, Func<Step, bool> predicate)
        {
            var index = IndexOf(step);
            if (index == -1)
                throw new ArgumentException($"The {step} step is not in the collection");
            var previousStep = Items.ElementAtOrDefault(--index);
            while (previousStep != null && !predicate.Invoke(previousStep))
                previousStep = Items.ElementAtOrDefault(--index);
            return previousStep;
        }

        public Step? NextOrDefault(Step step)
        {
            return NextOrDefault(step, x => true);
        }

        public Step? NextOrDefault(Step step, Func<Step, bool> predicate)
        {
            var index = IndexOf(step);
            if (index == -1)
                throw new ArgumentException($"The {step} step is not in the collection");
            var nextStep = Items.ElementAtOrDefault(++index);
            while (nextStep != null && !predicate.Invoke(nextStep))
                nextStep = Items.ElementAtOrDefault(++index);
            return nextStep;
        }
    }
}