using System;
using System.Linq;
using System.Threading.Tasks;

namespace Roro.Flows.Services
{
    public interface IFlowPickerService
    {
        public const char FOLDER_SEPARATOR = '/';

        public Task AddFileAsync(string filePath, string contents);

        public Task<bool> FileExistsAsync(string filePath);

        public Task<string> GetFileContentsAsync(string filePath);

        public Task SetFileContentsAsync(string filePath, string contents);
    }
}
