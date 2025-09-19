using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using AuctionSystem.Client;
using AuctionSystem.Client.Services;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// 🔗 API base URL — საჭიროებისამებრ შეცვალე
var apiBase = new Uri("https://localhost:7111/");

// HttpClient, რომელსაც ApiClient გამოიყენებს
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = apiBase });

// ✅ Register services (each once)
builder.Services.AddScoped<Storage>();
builder.Services.AddScoped<ApiClient>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ToastService>();

await builder.Build().RunAsync();
