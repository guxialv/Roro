using System.Threading.Tasks;

namespace Roro.Flows.Execution
{
    public interface IExecutable
    {
        internal Task<ExecutionResult> ExecuteAsync(ExecutionContext context);
    }
}