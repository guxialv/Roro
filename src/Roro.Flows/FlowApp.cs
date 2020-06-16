using Roro.Flows.Execution;
using Roro.Flows.Framework;
using Roro.Flows.Services;
using System.Threading.Tasks;

namespace Roro.Flows
{
    public sealed class FlowApp : ViewModel
    {
        private readonly CallStack _callStack;
        internal readonly ServiceCollection _services;

        public FlowApp()
        {
            _callStack = new CallStack(this);
            _services = new ServiceCollection();
            _services.Add<IFlowPickerService>(() => new FlowPickerService());
            Flows = new FlowCollection(this);
        }

        public FlowCollection Flows { get; }

        public Flow? CurrentFlow => _callStack.CurrentFlow;

        public Step? CurrentStep => _callStack.CurrentStep;

        public async Task RunAsync() => await _callStack.RunAsnyc();
    }
}
