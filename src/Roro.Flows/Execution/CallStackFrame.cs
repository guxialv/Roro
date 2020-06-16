using System.Collections.Generic;

namespace Roro.Flows.Execution
{
    public sealed class CallStackFrame
    {
        internal CallStackFrame(IExecutable executable)
        {
            Executable = executable;
            IsFirstEntry = true;
            AllowReentry = true;
            Locals = new Dictionary<string, object>();
            Inputs = new Dictionary<string, object>();
            Outputs = new Dictionary<string, object>();
        }

        internal IExecutable Executable { get; }

        internal bool IsFirstEntry { get; set; }

        internal bool AllowReentry { get; set; }

        internal Dictionary<string, object> Locals { get; }

        internal Dictionary<string, object> Inputs { get; }

        internal Dictionary<string, object> Outputs { get; }
    }
}
