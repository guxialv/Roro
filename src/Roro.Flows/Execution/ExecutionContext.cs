using System.Collections.Generic;

namespace Roro.Flows.Execution
{
    public sealed class ExecutionContext
    {
        private readonly CallStack _callStack;
        private readonly CallStackFrame _call;

        internal ExecutionContext(CallStack callStack, CallStackFrame call)
        {
            _callStack = callStack;
            _call = call;
        }

        public IExecutable Executable => _call.Executable;

        public bool IsFirstEntry => _call.IsFirstEntry;

        public bool AllowReentry => _call.AllowReentry;

        public Dictionary<string, object> Globals => _callStack.Globals;

        public Dictionary<string, object> Locals => _call.Locals;

        public Dictionary<string, object> Inputs => _call.Inputs;

        public Dictionary<string, object> Outputs => _call.Outputs;

        internal void PushCall(CallStackFrame call)
        {
            _callStack.Calls.Push(call);
        }

        internal CallStackFrame? PeekCall()
        {
            return _callStack.Calls.TryPeek(out CallStackFrame call) ? call : null;
        }

        internal CallStackFrame? PopCall()
        {
            return _callStack.Calls.TryPop(out CallStackFrame call) ? call : null;
        }
    }
}
