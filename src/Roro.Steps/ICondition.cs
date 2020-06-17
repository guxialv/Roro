using System.Threading.Tasks;

namespace Roro.Steps
{
    public interface ICondition
    {
        public Task<bool> ExecuteAsync(IExecutionContext context);
    }
}
