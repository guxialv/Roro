using Roro.Flows.Framework;

namespace Roro.Flows
{
    public sealed class StepOutput : NameTypeValue
    {
        public override bool NameIsReadOnly => true;

        public override bool TypeIsReadOnly => true;

        public override bool ValueIsReadOnly => false;
    }
}
