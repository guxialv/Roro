using Roro.Flows.Framework;
using Roro.Flows.Services;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Roro.Flows
{
    public sealed class FlowCollection : ViewModelCollection<FlowApp, FlowCollection, Flow>
    {
        internal FlowCollection(FlowApp parent) : base(parent)
        {
        }

        protected override Flow CreateItem(JsonElement jsonElement)
        {
            throw new NotSupportedException();
        }

        public async Task<Flow> AddNewAsync()
        {
            var flow = new Flow(Parent, $"/flow-{Count + 1}");
            await Parent.Services.GetShared<IFlowPickerService>()!.AddFileAsync(flow.Path, flow.ToJson());
            Add(flow);
            return flow;
        }

        public async Task SaveAllAsync()
        {
            foreach (var flow in Items)
            {
                await Parent.Services.GetShared<IFlowPickerService>()!.SetFileContentsAsync(flow.Path, flow.ToJson());
            }
        }
    }
}