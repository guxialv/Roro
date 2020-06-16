using Roro.Flows.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roro.Flows.Execution
{
    public sealed class CallStack
    {
        private readonly FlowApp _app;
        private readonly Stack<CallStackFrame> _calls;

        internal CallStack(FlowApp app)
        {
            _app = app;
            _calls = new Stack<CallStackFrame>();
            Globals = new Dictionary<string, object>();
        }

        internal Flow? CurrentFlow => _calls.Peek()?.Executable is Flow flow ? flow : CurrentStep?._parentStepCollection._parentFlow;

        internal Step? CurrentStep => _calls.Peek()?.Executable is Step step ? step : null;

        internal Dictionary<string, object> Globals { get; }

        internal void PushCall(CallStackFrame call)
        {
            _calls.Push(call);
            if (call.Executable is Flow flow)
            {
                _app.Flows.Add(flow);
            }
        }

        internal CallStackFrame? PeekCall()
        {
            return _calls.TryPeek(out CallStackFrame call) ? call : null;
        }

        internal CallStackFrame? PopCall()
        {
            if (_calls.TryPop(out CallStackFrame call))
            {
                if (call.Executable is Flow flow)
                {
                    _app.Flows.Remove(flow);
                }
            }
            return call;
        }

        internal bool CanRun
        {
            get => _calls.Count == 0;
        }

        internal async Task RunAsnyc()
        {
            await _app.Flows.SaveAllAsync();

            var path = _app.Flows.First().Path;
            var json = await _app._services.GetShared<IFlowPickerService>()!.GetFileContentsAsync(path);
            var flow = new Flow(_app.Flows, path, JsonDocument.Parse(json).RootElement);

            _calls.Clear();
            _calls.Push(new CallStackFrame(flow));
            Console.WriteLine($"INFO: Execution started.");
            while (_calls.TryPeek(out CallStackFrame call))
            {
                try
                {
                    Console.WriteLine($"INFO: Executing {call.Executable.GetType().Name}..");
                    var context = new ExecutionContext(this, call);
                    var result = await call.Executable.ExecuteAsync(context);
                    call.IsFirstEntry = false;
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"FAIL: {exception.Message}");
                    Console.WriteLine($"FAIL: Execution failed.");

                    while (PopCall() != null) ;
                }
            }
            Console.WriteLine($"INFO: Execution completed.");
        }
    }
}
