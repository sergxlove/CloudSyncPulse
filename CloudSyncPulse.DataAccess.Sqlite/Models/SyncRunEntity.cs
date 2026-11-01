using CloudSyncPulse.Core.Enums;

namespace CloudSyncPulse.DataAccess.Sqlite.Models
{
    public class SyncRunEntity
    {
        public Guid Id { get; set; }
        public Guid SyncRuleId { get; set; }
        public DateTime StartedUtc { get; set; }
        public DateTime? FinishedUtc { get; set; }
        public SyncRunStatus Status { get; set; }
        public SyncRunTrigger Trigger { get; set; }
        public int FilesTotal { get; set; }
        public int FilesSucceeded { get; set; }
        public int FilesFailed { get; set; }
        public long BytesUploaded { get; set; }
        public string? ErrorMessage { get; set; }

        public SyncRulesEntity SyncRule { get; set; } = null!;
    }
}
