using Roro.Flows.Execution;
using Roro.Flows.Framework;
using Roro.Flows.Steps;
using System;
using System.Collections;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roro.Flows
{
    public abstract class Step : ViewModel<Flow, StepCollection, Step>, IExecutable
    {
        internal Step? ParentStep => ParentCollection?.ParentStep;

        protected Step(Flow parent) : base(parent)
        {
        }

        protected Step(Flow parent, JsonElement jsonElement) : base(parent)
        {
            if (jsonElement.TryGetProperty(nameof(Call), out JsonElement jsonProperty))
            {
                Call = jsonProperty.GetString();
            }
            if (jsonElement.TryGetProperty(nameof(Inputs), out jsonProperty))
            {
                Inputs = new StepInputCollection(this, jsonProperty);
            }
            if (jsonElement.TryGetProperty(nameof(Outputs), out jsonProperty))
            {
                Outputs = new StepOutputCollection(this, jsonProperty);
            }
            if (jsonElement.TryGetProperty(nameof(Steps), out jsonProperty))
            {
                Steps = new StepCollection(Parent, this, jsonProperty);
            }
        }

        internal static Step Create<TStep>(Flow parent) where TStep : Step
        {
            var type = typeof(TStep).Name;
            if (type == typeof(ActionStep).Name)
                return new ActionStep(parent);
            if (type == typeof(IfStep).Name)
                return new IfStep(parent);
            if (type == typeof(ElseIfStep).Name)
                return new ElseIfStep(parent);
            if (type == typeof(ElseStep).Name)
                return new ElseStep(parent);
            if (type == typeof(ForStep).Name)
                return new ForStep(parent);
            if (type == typeof(WhileStep).Name)
                return new WhileStep(parent);
            if (type == typeof(BreakStep).Name)
                return new BreakStep(parent);
            if (type == typeof(ContinueStep).Name)
                return new ContinueStep(parent);
            if (type == typeof(TryStep).Name)
                return new TryStep(parent);
            if (type == typeof(CatchStep).Name)
                return new CatchStep(parent);
            if (type == typeof(ThrowStep).Name)
                return new ThrowStep(parent);
            if (type == typeof(CommentStep).Name)
                return new CommentStep(parent);
            if (type == typeof(FlowStep).Name)
                return new FlowStep(parent);
            throw new NotSupportedException();
        }

        internal static Step Create(Flow parent, JsonElement jsonElement)
        {
            var type = jsonElement.GetProperty(nameof(Type)).GetString();
            if (type == typeof(ActionStep).Name)
                return new ActionStep(parent, jsonElement);
            if (type == typeof(IfStep).Name)
                return new IfStep(parent, jsonElement);
            if (type == typeof(ElseIfStep).Name)
                return new ElseIfStep(parent, jsonElement);
            if (type == typeof(ElseStep).Name)
                return new ElseStep(parent, jsonElement);
            if (type == typeof(ForStep).Name)
                return new ForStep(parent, jsonElement);
            if (type == typeof(WhileStep).Name)
                return new WhileStep(parent, jsonElement);
            if (type == typeof(BreakStep).Name)
                return new BreakStep(parent, jsonElement);
            if (type == typeof(ContinueStep).Name)
                return new ContinueStep(parent, jsonElement);
            if (type == typeof(TryStep).Name)
                return new TryStep(parent, jsonElement);
            if (type == typeof(CatchStep).Name)
                return new CatchStep(parent, jsonElement);
            if (type == typeof(ThrowStep).Name)
                return new ThrowStep(parent, jsonElement);
            if (type == typeof(CommentStep).Name)
                return new CommentStep(parent, jsonElement);
            if (type == typeof(FlowStep).Name)
                return new FlowStep(parent, jsonElement);
            throw new NotSupportedException();
        }

        internal override void ToJson(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteString(nameof(Type), Type);
            if (Call != null)
            {
                writer.WriteString(nameof(Call), Call);
            }
            if (Inputs != null)
            {
                writer.WritePropertyName(nameof(Inputs));
                Inputs.ToJson(writer);
            }
            if (Outputs != null)
            {
                writer.WritePropertyName(nameof(Outputs));
                Outputs.ToJson(writer);
            }
            if (Steps != null)
            {
                writer.WritePropertyName(nameof(Steps));
                Steps.ToJson(writer);
            }
            writer.WriteEndObject();
        }

        #region IExecutable

        public string Path => Parent.Path + ParentStep?.Path + "/" + (ParentCollection!.IndexOf(this) + 1);

        public string Type => GetType().Name;

        public virtual string? Call { get; set; }

        public StepInputCollection? Inputs { get; protected set; }

        public StepOutputCollection? Outputs { get; protected set; }

        public StepCollection? Steps { get; protected set; }

        IEnumerable? IExecutable.Inputs => throw new NotImplementedException();

        IEnumerable? IExecutable.Outputs => throw new NotImplementedException();

        async Task<ExecutionResult> IExecutable.ExecuteAsync(ExecutionContext context) => await ExecuteAsync(context);

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
                    if (tryStep.ParentCollection!.NextOrDefault(tryStep) is Step nextStep)
                    {
                        context.PushCall(new CallStackFrame(nextStep));
                    }
                    break;
                }
            }
        }

        #endregion
    }
}
