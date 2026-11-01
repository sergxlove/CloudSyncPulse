using CloudSyncPulse.Core.Infrastructures;

namespace CloudSyncPulse.Core.Models
{
    public class ProviderCredential
    {
        public Guid ProviderId { get; private set; }
        public byte[] AccessTokenEncrypted { get; private set; } = Array.Empty<byte>();
        public byte[]? RefreshTokenEncrypted { get; private set; }
        public DateTime? ExpiresAtUtc { get; private set; }
        public string? Scope { get; private set; }
        public string? TokenType { get; private set; }
        public DateTime UpdatedUtc { get; private set; } = DateTime.UtcNow;

        private ProviderCredential() { }

        public static ResModel<ProviderCredential> Create(
            Guid providerId,
            byte[] accessTokenEncrypted,
            byte[]? refreshTokenEncrypted = null,
            DateTime? expiresAtUtc = null,
            string? scope = null,
            string? tokenType = null)
        {
            if (providerId == Guid.Empty)
                return new ResModel<ProviderCredential>("ProviderId не может быть пустым.");

            if (accessTokenEncrypted is null || accessTokenEncrypted.Length == 0)
                return new ResModel<ProviderCredential>("AccessTokenEncrypted обязателен.");

            if (expiresAtUtc.HasValue && expiresAtUtc.Value.Kind != DateTimeKind.Utc)
                return new ResModel<ProviderCredential>("ExpiresAtUtc должен быть в UTC.");

            return new ResModel<ProviderCredential>(new ProviderCredential
            {
                ProviderId = providerId,
                AccessTokenEncrypted = accessTokenEncrypted,
                RefreshTokenEncrypted = refreshTokenEncrypted,
                ExpiresAtUtc = expiresAtUtc,
                Scope = scope,
                TokenType = tokenType,
                UpdatedUtc = DateTime.UtcNow
            });
        }
    }
}
