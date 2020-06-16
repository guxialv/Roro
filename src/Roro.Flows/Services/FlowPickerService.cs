using System.Collections.Generic;
using System.Threading.Tasks;

namespace Roro.Flows.Services
{
    public sealed class FlowPickerService : IFlowPickerService
    {
        private readonly Dictionary<string, string> _files;

        public FlowPickerService()
        {
            _files = new Dictionary<string, string>();
        }

        public async Task AddFileAsync(string filePath, string contents)
        {
            await Task.CompletedTask;
            _files.Add(filePath, contents);
        }

        public async Task<bool> FileExistsAsync(string filePath)
        {
            await Task.CompletedTask;
            return _files.ContainsKey(filePath);
        }

        public async Task<string> GetFileContentsAsync(string filePath)
        {
            await Task.CompletedTask;
            return _files[filePath];
        }

        public async Task SetFileContentsAsync(string filePath, string contents)
        {
            await Task.CompletedTask;
            _files[filePath] = contents;
        }
    }
}
