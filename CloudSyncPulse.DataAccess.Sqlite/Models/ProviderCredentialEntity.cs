namespace CloudSyncPulse.DataAccess.Sqlite.Models
{
    public class ProviderCredentialEntity
    {
        public Guid ProviderId { get; set; }    
        public byte[] AccessTokenEncrypted { get; set; } = Array.Empty<byte>();
        public byte[]? RefreshTokenEncrypted { get; set; }
        public DateTime? ExpiresAtUtc { get; set; }
        public string? Scope { get; set; }
        public string? TokenType { get; set; }
        public DateTime UpdatedUtc { get; set; }

        public ProvidersEntity Provider { get; set; } = null!;
    }
}
