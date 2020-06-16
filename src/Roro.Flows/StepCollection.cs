using Roro.Flows.Framework;
using Roro.Flows.Steps;
using System.Linq;
using System.Text.Json;

namespace Roro.Flows
{
    public sealed class StepCollection : ViewModelCollection<Step>
    {
        internal readonly Flow _parentFlow;
        internal readonly ParentStep? _parentStep;

        internal StepCollection(Flow parentFlow)
        {
            _parentFlow = parentFlow;
        }

        internal StepCollection(Flow parentFlow, JsonElement jsonElement) : this(parentFlow)
        {
            jsonElement.EnumerateArray().ToList().ForEach(step => Add(Step.Create(this, step)));
        }

        internal StepCollection(ParentStep parentStep)
            : this(parentStep._parentStepCollection._parentFlow)
        {
            _parentStep = parentStep;
        }

        internal StepCollection(ParentStep parentStep, JsonElement jsonElement)
            : this(parentStep._parentStepCollection._parentFlow, jsonElement)
        {
        }

        public ActionStep AddAction()
        {
            var step = new ActionStep(this);
            Add(step);
            return step;
        }

        public IfStep AddIf()
        {
            var step = new IfStep(this);
            Add(step);
            return step;
        }

        public ElseIfStep AddElseIf()
        {
            var step = new ElseIfStep(this);
            Add(step);
            return step;
        }

        public ElseStep AddElse()
        {
            var step = new ElseStep(this);
            Add(step);
            return step;
        }

        public ForStep AddFor()
        {
            var step = new ForStep(this);
            Add(step);
            return step;
        }

        public WhileStep AddWhile()
        {
            var step = new WhileStep(this);
            Add(step);
            return step;
        }

        public BreakStep AddBreak()
        {
            var step = new BreakStep(this);
            Add(step);
            return step;
        }

        public ContinueStep AddContinue()
        {
            var step = new ContinueStep(this);
            Add(step);
            return step;
        }

        public TryStep AddTry()
        {
            var step = new TryStep(this);
            Add(step);
            return step;
        }

        public CatchStep AddCatch()
        {
            var step = new CatchStep(this);
            Add(step);
            return step;
        }

        public ThrowStep AddThrow()
        {
            var step = new ThrowStep(this);
            Add(step);
            return step;
        }

        public CallStep AddCall()
        {
            var step = new CallStep(this);
            Add(step);
            return step;
        }
    }
}