using Roro.Flows.Execution;
using Roro.Flows.Framework;
using Roro.Flows.Services;
using System.Threading.Tasks;

namespace Roro.Flows
{
    public sealed class FlowApp : ViewModel
    {
        private readonly CallStack _callStack;

        public FlowApp()
        {
            _callStack = new CallStack(this);
            Services = new ServiceCollection();
            Flows = new FlowCollection(this);
            UseDefaultServices();
        }

        private void UseDefaultServices()
        {
            Services.Add<IFlowPickerService>(() => new FlowPickerService());
        }

        public ServiceCollection Services { get; }

        public FlowCollection Flows { get; }

        public Flow? CurrentFlow => _callStack.CurrentFlow;

        public Step? CurrentStep => _callStack.CurrentStep;

        public async Task RunAsync() => await _callStack.RunAsnyc();
    }
}
