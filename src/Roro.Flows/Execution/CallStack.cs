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

        internal CallStack(FlowApp app)
        {
            _app = app;
            Calls = new Stack<CallStackFrame>();
            Globals = new Dictionary<string, object>();
        }

        public Stack<CallStackFrame> Calls { get; }

        public Dictionary<string, object> Globals { get; }

        public async Task RunAsnyc()
        {
            await _app.Flows.SaveAllAsync();

            var path = _app.Flows.First().Path;
            var json = await _app._services.GetShared<IFlowPickerService>()!.GetFileContentsAsync(path);
            var flow = new Flow(_app.Flows, path, JsonDocument.Parse(json).RootElement);

            Calls.Clear();
            Calls.Push(new CallStackFrame(flow));
            Console.WriteLine($"INFO: Execution started.");
            while (Calls.TryPeek(out CallStackFrame call))
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
                    Console.WriteLine($"FAIL: Execution terminated.");
                    return;
                }
            }
            Console.WriteLine($"INFO: Execution completed.");
        }
    }
}
