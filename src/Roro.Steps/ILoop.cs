using System.Threading.Tasks;

namespace Roro.Steps
{
    public interface ILoop
    {
        public Task<bool> ExecuteAsync(IExecutionContext context, bool isFirstEntry);
    }
}
