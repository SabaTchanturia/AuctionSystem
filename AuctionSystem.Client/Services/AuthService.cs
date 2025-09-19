using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuctionSystem.Client.Services
{
    /// <summary>
    /// Keeps JWT in storage, exposes auth state, parses UserId from the token.
    /// </summary>
    public class AuthService
    {
        private readonly Storage _storage;
        private readonly HttpClient _http;

        private const string TokenKey = "auth_token";
        private const string UsernameKey = "auth_username";

        public bool IsAuthenticated => !string.IsNullOrEmpty(Token);
        public string? Username { get; private set; }
        public string? Token { get; private set; }

        // parsed from JWT
        public int? UserId { get; private set; }

        public AuthService(Storage storage, HttpClient http)
        {
            _storage = storage;
            _http = http;
        }

        public async Task InitializeAsync()
        {
            Token = await _storage.Get(TokenKey);
            Username = await _storage.Get(UsernameKey);

            if (!string.IsNullOrEmpty(Token))
            {
                ParseClaimsFromToken(Token!);
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Token);
            }
        
        }

        public async Task<bool> Register(string username, string password)
        {
            var payload = new
            {
                username = username,
                passwordHash = password
            };

            var json = JsonSerializer.Serialize(payload);
            var resp = await _http.PostAsync("api/Auth/register",
                new StringContent(json, Encoding.UTF8, "application/json"));

            return resp.IsSuccessStatusCode;
        }



        public async Task<bool> Login(string username, string password)
        {
            var payload = new
            {
                username = username,
                passwordHash = password
            };

            var json = JsonSerializer.Serialize(payload);
            var resp = await _http.PostAsync("api/Auth/login",
                new StringContent(json, Encoding.UTF8, "application/json"));

            if (!resp.IsSuccessStatusCode) return false;

            var text = await resp.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(text);
            if (!doc.RootElement.TryGetProperty("token", out var tokenEl)) return false;

            Token = tokenEl.GetString();
            Username = username;

            if (!string.IsNullOrEmpty(Token))
            {
                ParseClaimsFromToken(Token!);

                await _storage.Set(TokenKey, Token!);
                await _storage.Set(UsernameKey, Username ?? "");

                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Token);
            }

            return true;
        }

        public async Task Logout()
        {
            Token = null;
            Username = null;
            UserId = null;

            _http.DefaultRequestHeaders.Authorization = null;

            await _storage.Remove(TokenKey);
            await _storage.Remove(UsernameKey);
        }

        private void ParseClaimsFromToken(string token)
        {
            try
            {
                var parts = token.Split('.');
                if (parts.Length < 2) return;

                // Base64Url decode (simple padding to avoid % error)
                string payload = parts[1].Replace('-', '+').Replace('_', '/');
                while (payload.Length % 4 != 0) payload += "=";

                var jsonBytes = Convert.FromBase64String(payload);
                var json = Encoding.UTF8.GetString(jsonBytes);

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                // common claim keys for user id
                var keys = new[]
                {
                    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                    "nameid", "sub", "uid", "userId"
                };

                foreach (var k in keys)
                {
                    if (root.TryGetProperty(k, out var v))
                    {
                        // claim can be string or number
                        if (v.ValueKind == JsonValueKind.String)
                        {
                            var s = v.GetString();
                            if (!string.IsNullOrEmpty(s) && int.TryParse(s, out var id))
                            {
                                UserId = id;
                                break;
                            }
                        }
                        else if (v.ValueKind == JsonValueKind.Number && v.TryGetInt32(out var n))
                        {
                            UserId = n;
                            break;
                        }
                    }
                }
            }
            catch
            {
                // ignore parse errors
            }
        }
    }
}
