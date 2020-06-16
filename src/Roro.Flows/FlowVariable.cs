using Roro.Flows.Framework;

namespace Roro.Flows
{
    public sealed class FlowVariable : NameTypeValue
    {
        public override bool NameIsReadOnly => false;

        public override bool TypeIsReadOnly => false;

        public override bool ValueIsReadOnly => false;
    }
}
