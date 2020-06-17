using Roro.Flows.Framework;
using Roro.Flows.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Roro.Flows
{
    public sealed class StepCollection : ViewModelCollection<Flow, StepCollection, Step>
    {
        internal ParentStep? ParentStep { get; }

        internal StepCollection(Flow parent, ParentStep? parentStep) : base(parent)
        {
            ParentStep = parentStep;
        }

        internal StepCollection(Flow parent, ParentStep? parentStep, JsonElement jsonElement) : base(parent, jsonElement)
        {
            ParentStep = parentStep;
        }

        protected override Step CreateItem(JsonElement jsonElement)
        {
            return Step.Create(Parent, jsonElement);
        }

        protected override void InsertItem(int index, Step item)
        {
            if (index == 0)
                AddFirst(item);
            else if (index == Count)
                AddLast(item);
            else
                AddBefore(Items[index], item);
        }

        protected override void RemoveItem(int index)
        {
            var indexes = new List<int>();
            var item = Items.ElementAt(index);
            indexes.Add(index);
            if (item is IfStep)
            {
                item = Items.ElementAtOrDefault(++index);
                while (item is ElseIfStep || item is ElseStep)
                {
                    indexes.Add(index);
                    item = Items.ElementAtOrDefault(++index);
                }
            }
            else if (item is TryStep)
            {
                item = Items.ElementAtOrDefault(++index);
                if (item is CatchStep)
                {
                    indexes.Add(index);
                }
            }
            indexes.ForEach(index => base.RemoveItem(index));
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

        public bool CanAddAfter(Step step, Step newNextStep, out Exception? exception)
        {
            exception = null;
            if (newNextStep is BreakStep || newNextStep is ContinueStep)
            {
                if (ParentOrDefault(step, parentStep => parentStep is ForStep || parentStep is WhileStep) is Step)
                    return true;
                exception = new Exception($"{newNextStep.Type} should be inside {typeof(ForStep).Name} or {typeof(WhileStep).Name}");
            }
            else if (newNextStep is ElseIfStep || newNextStep is ElseStep)
            {
                if (step is IfStep || step is ElseIfStep)
                    return true;
                exception = new Exception($"{newNextStep.Type} should be after {typeof(IfStep).Name} or {typeof(ElseIfStep).Name}");
            }
            else if (newNextStep is CatchStep)
            {
                if (step is TryStep)
                    return true;
                exception = new Exception($"{newNextStep.Type} should be after {typeof(TryStep).Name}");
            }
            return exception is null;
        }

        public void AddAfter(Step step, Step newNextStep)
        {
            if (!CanAddAfter(step, newNextStep, out Exception? exception))
                throw exception!;
            base.InsertItem(IndexOf(step) + 1, newNextStep);
        }

        public bool CanAddBefore(Step step, Step newPreviousStep, out Exception? exception)
        {
            if (PreviousOrDefault(step) is Step previousStep)
                return CanAddAfter(previousStep, newPreviousStep, out exception);
            else
                return CanAddFirst(step, out exception);
        }

        public void AddBefore(Step step, Step newPreviousStep)
        {
            if (!CanAddBefore(step, newPreviousStep, out Exception? exception))
                throw exception!;
            base.InsertItem(IndexOf(step), newPreviousStep);
        }

        public bool CanAddFirst(Step newFirstStep, out Exception? exception)
        {
            exception = null;
            if (newFirstStep is BreakStep || newFirstStep is ContinueStep)
            {
                if (ParentStep is ForStep || ParentStep is WhileStep)
                    return true;
                if (ParentStep is Step && ParentOrDefault(ParentStep, parentStep => parentStep is ForStep || parentStep is WhileStep) is Step)
                    return true;
                exception = new Exception($"{newFirstStep.Type} should be inside {typeof(ForStep).Name} or {typeof(WhileStep).Name}");
            }
            else if (newFirstStep is ElseIfStep || newFirstStep is ElseStep)
                exception = new Exception($"{newFirstStep.Type} should be after {typeof(IfStep).Name} or {typeof(ElseIfStep).Name}");
            else if (newFirstStep is CatchStep)
                exception = new Exception($"{newFirstStep.Type} should be after {typeof(TryStep).Name}");
            return exception is null;
        }

        public void AddFirst(Step newFirstStep)
        {
            if (!CanAddFirst(newFirstStep, out Exception? exception))
                throw exception!;
            base.InsertItem(0, newFirstStep);
        }

        public bool CanAddLast(Step newLastStep, out Exception? exception)
        {
            if (Items.LastOrDefault() is Step lastStep)
                return CanAddAfter(lastStep, newLastStep, out exception);
            else
                return CanAddFirst(newLastStep, out exception);
        }

        public void AddLast(Step newLastStep)
        {
            if (!CanAddLast(newLastStep, out Exception? exception))
                throw exception!;
            base.InsertItem(Count, newLastStep);
        }

        public ParentStep? ParentOrDefault(Step step)
        {
            return ParentOrDefault(step, x => true);
        }

        public ParentStep? ParentOrDefault(Step step, Func<Step, bool> predicate)
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