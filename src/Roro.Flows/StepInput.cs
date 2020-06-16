using Roro.Flows.Framework;

namespace Roro.Flows
{
    public sealed class StepInput : NameTypeValue
    {
        internal readonly StepInputCollection _parentStepInputCollection;

        internal StepInput(StepInputCollection parentStepInputCollection)
        {
            _parentStepInputCollection = parentStepInputCollection;
        }

        public override bool NameIsReadOnly => true;

        public override bool TypeIsReadOnly => true;

        public override bool ValueIsReadOnly => false;
    }
}
