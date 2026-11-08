using CloudSyncPulse.Core.Enums;

namespace CloudSyncPulse.Web.Requests
{
    public class CreateProviderRequest
    {
        public ProviderType Type { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? ExpiresAtUtc { get; set; }
        public string? Scope { get; set; }
        public string? TokenType { get; set; }
    }

    public class RenameProviderRequest
    {
        public string DisplayName { get; set; } = string.Empty;
    }

    public class SetEnabledRequest
    {
        public bool Enabled { get; set; }
    }
}
