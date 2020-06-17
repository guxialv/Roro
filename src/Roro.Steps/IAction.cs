using System.Threading.Tasks;

namespace Roro.Steps
{
    public interface IAction
    {
        public Task ExecuteAsync(IExecutionContext context);
    }
}
