using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace AuctionSystem.Client.Services
{
    /// <summary>
    /// Simple wrapper over browser localStorage for Blazor WASM.
    /// </summary>
    public class Storage
    {
        private readonly IJSRuntime _js;

        public Storage(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<string?> Get(string key)
        {
            return await _js.InvokeAsync<string?>("localStorage.getItem", key);
        }

        public async Task Set(string key, string value)
        {
            await _js.InvokeVoidAsync("localStorage.setItem", key, value);
        }

        public async Task Remove(string key)
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", key);
        }
    }
}
