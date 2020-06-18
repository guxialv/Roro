using System.Collections;
using System.Threading.Tasks;

namespace Roro.Flows.Execution
{
    public interface IExecutable
    {
        public string Path { get; }

        public string Type { get; }

        public string? SubType { get; }

        public IEnumerable? Inputs { get; }

        public IEnumerable? Outputs { get; }

        internal Task<ExecutionResult> ExecuteAsync(ExecutionContext context);
    }
}