using System.Threading.Tasks;

namespace Roro.Flows.Services
{
    public interface IUserDialogService
    {
        public Task AlertAsync(string message);

        public Task<string?> PromptAsync(string message, string defaultValue);

        public Task<bool> ConfirmAsync(string message);
    }
}
