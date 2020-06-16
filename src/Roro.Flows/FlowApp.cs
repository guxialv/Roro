using Roro.Flows.Execution;
using Roro.Flows.Framework;
using Roro.Flows.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Roro.Flows
{
    public sealed class FlowApp : ViewModel
    {
        internal readonly ServiceCollection _services;
        private readonly CallStack _callStack;

        public FlowApp()
        {
            _callStack = new CallStack(this);
            _services = new ServiceCollection();
            _services.Add<IFlowPickerService>(() => new FlowPickerService());
            Flows = new FlowCollection(this);
        }

        public FlowCollection Flows { get; }

        public Flow? CurrentFlow => _callStack.Calls.Peek()?.Executable
                                    is Flow flow ? flow : CurrentStep?._parentStepCollection._parentFlow;

        public Step? CurrentStep => _callStack.Calls.Peek()?.Executable
                                    is Step step ? step : null;

        public async Task RunAsync() => await _callStack.RunAsnyc();
    }
}
