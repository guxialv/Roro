using Roro.Flows.Framework;

namespace Roro.Flows
{
    public sealed class FlowOutput : NameTypeValue
    {
        public override bool NameIsReadOnly => true;

        public override bool TypeIsReadOnly => true;

        public override bool ValueIsReadOnly => false;
    }
}
