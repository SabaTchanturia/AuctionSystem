using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Client.Services
{
    /// <summary>
    /// Simple HTTP helper: attaches JWT and provides JSON helpers.
    /// </summary>
    public class ApiClient
    {
        private readonly HttpClient _http;
        private readonly AuthService _auth;

        public ApiClient(HttpClient http, AuthService auth)
        {
            _http = http;
            _auth = auth;
        }

        private async Task EnsureAuthHeaderAsync()
        {
            await _auth.InitializeAsync(); // idempotent
            if (_auth.IsAuthenticated && !_http.DefaultRequestHeaders.Contains("Authorization"))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _auth.Token);
            }
        }

        public async Task<T?> GetJson<T>(string url)
        {
            await EnsureAuthHeaderAsync();
            return await _http.GetFromJsonAsync<T>(url);
        }

        public async Task<HttpResponseMessage> PostJson<T>(string url, T payload)
        {
            await EnsureAuthHeaderAsync();
            return await _http.PostAsJsonAsync(url, payload);
        }

        public async Task<HttpResponseMessage> PostRaw(string url, string raw, string contentType = "application/json")
        {
            await EnsureAuthHeaderAsync();
            var content = new StringContent(raw, Encoding.UTF8, contentType);
            return await _http.PostAsync(url, content);
        }

        // NEW: used by AuctionDetails.razor OnDelete
        public async Task<HttpResponseMessage> Delete(string url)
        {
            await EnsureAuthHeaderAsync();
            return await _http.DeleteAsync(url);    
        }
    }
}
