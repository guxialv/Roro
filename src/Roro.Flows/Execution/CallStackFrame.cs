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

        public IExecutable Executable { get; }

        public bool IsFirstEntry { get; set; }

        public bool AllowReentry { get; set; }

        public Dictionary<string, object> Locals { get; }

        public Dictionary<string, object> Inputs { get; }

        public Dictionary<string, object> Outputs { get; }
    }
}
