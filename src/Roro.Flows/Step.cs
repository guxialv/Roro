using Roro.Flows.Execution;
using Roro.Flows.Framework;
using Roro.Flows.Steps;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roro.Flows
{
    public abstract class Step : ViewModel, IExecutable
    {
        internal readonly StepCollection _parentStepCollection;

        protected Step(StepCollection parentStepCollection)
        {
            _parentStepCollection = parentStepCollection;
        }

        internal static Step Create(StepCollection parentStepCollection, JsonElement jsonElement)
        {
            var type = jsonElement.GetProperty(nameof(Type)).GetString();
            if (type == typeof(ActionStep).Name)
                return new ActionStep(parentStepCollection, jsonElement);
            if (type == typeof(IfStep).Name)
                return new IfStep(parentStepCollection, jsonElement);
            if (type == typeof(ElseIfStep).Name)
                return new ElseIfStep(parentStepCollection, jsonElement);
            if (type == typeof(ElseStep).Name)
                return new ElseStep(parentStepCollection, jsonElement);
            if (type == typeof(ForStep).Name)
                return new ForStep(parentStepCollection, jsonElement);
            if (type == typeof(WhileStep).Name)
                return new WhileStep(parentStepCollection, jsonElement);
            if (type == typeof(BreakStep).Name)
                return new BreakStep(parentStepCollection, jsonElement);
            if (type == typeof(ContinueStep).Name)
                return new ContinueStep(parentStepCollection, jsonElement);
            if (type == typeof(TryStep).Name)
                return new TryStep(parentStepCollection, jsonElement);
            if (type == typeof(CatchStep).Name)
                return new CatchStep(parentStepCollection, jsonElement);
            if (type == typeof(ThrowStep).Name)
                return new ThrowStep(parentStepCollection, jsonElement);
            if (type == typeof(CallStep).Name)
                return new CallStep(parentStepCollection, jsonElement);
            throw new NotSupportedException();
        }

        public override void ToJson(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteString(nameof(Type), Type);
            writer.WriteEndObject();
        }

        public string Type => GetType().Name;

        public string Number => _parentStepCollection._parentStep is null ?
                                                        string.Empty + _parentStepCollection.IndexOf(this) :
                                _parentStepCollection._parentStep.Number + _parentStepCollection.IndexOf(this);

        protected Step? GetNextStep()
        {
            return GetNextStep(x => true);
        }

        protected Step? GetNextStep(Func<Step?, bool> predicate)
        {
            var index = _parentStepCollection.IndexOf(this);
            if (index == -1) throw new Exception();
            var nextStep = _parentStepCollection.ElementAtOrDefault(++index);
            while (nextStep != null && !predicate.Invoke(nextStep))
                nextStep = _parentStepCollection.ElementAtOrDefault(++index);
            return nextStep;
        }

        Task<ExecutionResult> IExecutable.ExecuteAsync(ExecutionContext context) => ExecuteAsync(context);

        protected abstract Task<ExecutionResult> ExecuteAsync(ExecutionContext context);

        protected async Task ThrowAsync(ExecutionContext context, Exception exception)
        {
            await Task.CompletedTask;
            context.PopCall();
            while (true)
            {
                var call = context.PopCall();
                if (call is null)
                {
                    throw exception;
                }
                if (call.Executable is TryStep tryStep)
                {
                    if (tryStep.GetNextStep() is Step nextStep)
                    {
                        context.PushCall(new CallStackFrame(nextStep));
                    }
                    break;
                }
            }
        }
    }
}
