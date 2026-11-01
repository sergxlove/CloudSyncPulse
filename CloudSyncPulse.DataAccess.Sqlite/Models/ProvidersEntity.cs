using CloudSyncPulse.Core.Enums;

namespace CloudSyncPulse.DataAccess.Sqlite.Models
{
    public class ProvidersEntity
    {
        public Guid Id { get; set; }
        public ProviderType Type { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime UpdatedUtc { get; set; }

        public ProviderCredentialEntity? Credential { get; set; }  
        public ICollection<SyncRulesEntity> SyncRules { get; set; } = new List<SyncRulesEntity>();
    }
}
