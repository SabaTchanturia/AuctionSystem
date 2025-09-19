using System.Collections.Concurrent;

namespace AuctionSystem.Client.Services
{
    public class ToastMessage
    {
        public string Text { get; set; } = "";
        public string Type { get; set; } = "info"; // info | success | danger | warning
        public int DurationMs { get; set; } = 3000;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class ToastService
    {
        public event Action<ToastMessage>? OnPush;

        public void Info(string text, int ms = 3000) => OnPush?.Invoke(new ToastMessage { Text = text, Type = "info", DurationMs = ms });
        public void Success(string text, int ms = 3000) => OnPush?.Invoke(new ToastMessage { Text = text, Type = "success", DurationMs = ms });
        public void Error(string text, int ms = 3000) => OnPush?.Invoke(new ToastMessage { Text = text, Type = "danger", DurationMs = ms });
        public void Warn(string text, int ms = 3000) => OnPush?.Invoke(new ToastMessage { Text = text, Type = "warning", DurationMs = ms });
    }
}
