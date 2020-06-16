using Roro.Flows.Framework;
using Roro.Flows.Services;
using System.Threading.Tasks;

namespace Roro.Flows
{
    public sealed class FlowCollection : ViewModelCollection<Flow>
    {
        internal readonly FlowApp _app;

        internal FlowCollection(FlowApp app)
        {
            _app = app;
        }

        public async Task<Flow> AddNewAsync()
        {
            var flow = new Flow(this, $"/flow-{Count}");
            await _app._services.GetShared<IFlowPickerService>()!.AddFileAsync(flow.Path, flow.ToJson());
            Add(flow);
            return flow;
        }

        public async Task SaveAllAsync()
        {
            foreach (var flow in Items)
            {
                await _app._services.GetShared<IFlowPickerService>()!.SetFileContentsAsync(flow.Path, flow.ToJson());
            }
        }
    }
}