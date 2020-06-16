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

        internal IExecutable Executable => _call.Executable;

        internal bool IsFirstEntry => _call.IsFirstEntry;

        internal bool AllowReentry => _call.AllowReentry;

        internal Dictionary<string, object> Globals => _callStack.Globals;

        internal Dictionary<string, object> Locals => _call.Locals;

        internal Dictionary<string, object> Inputs => _call.Inputs;

        internal Dictionary<string, object> Outputs => _call.Outputs;

        internal void PushCall(CallStackFrame call) => _callStack.PushCall(call);

        internal CallStackFrame? PeekCall() => _callStack.PeekCall();

        internal CallStackFrame? PopCall() => _callStack.PopCall();
    }
}
