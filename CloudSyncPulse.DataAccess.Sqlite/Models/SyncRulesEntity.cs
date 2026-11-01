using CloudSyncPulse.Core.Enums;

namespace CloudSyncPulse.DataAccess.Sqlite.Models
{
    public class SyncRulesEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool Enabled { get; set; }
        public string LocalPath { get; set; } = string.Empty;
        public LocalPathType LocalPathType { get; set; }
        public bool Recursive { get; set; }
        public Guid ProviderId { get; set; }
        public string RemoteFolder { get; set; } = "/";
        public string? RemoteFileNameTemplate { get; set; }
        public OverwritePolicy OverwritePolicy { get; set; }
        public bool DeleteAfterUpload { get; set; }
        public string Cron { get; set; } = "0 0 * * *";
        public DateTime? LastRunUtc { get; set; }
        public DateTime? NextRunUtc { get; set; }
        public bool IsDeleted { get; set; }

        public ProvidersEntity Provider { get; set; } = null!;                     
        public ICollection<SyncRunEntity> Runs { get; set; } = new List<SyncRunEntity>();
    }
}
