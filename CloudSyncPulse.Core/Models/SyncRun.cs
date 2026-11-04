using CloudSyncPulse.Core.Enums;
using CloudSyncPulse.Core.Infrastructures;

namespace CloudSyncPulse.Core.Models
{
    public class SyncRun
    {
        public Guid Id { get; private set; }
        public Guid SyncRuleId { get; private set; }
        public DateTime StartedUtc { get; private set; }
        public DateTime? FinishedUtc { get; private set; }
        public SyncRunStatus Status { get; private set; }
        public SyncRunTrigger Trigger { get; private set; }
        public int FilesTotal { get; private set; }
        public int FilesSucceeded { get; private set; }
        public int FilesFailed { get; private set; }
        public long BytesUploaded { get; private set; }
        public string? ErrorMessage { get; private set; }

        private SyncRun() { }

        public static ResModel<SyncRun> Create(Guid syncRuleId, SyncRunTrigger trigger)
        {
            if (syncRuleId == Guid.Empty)
                return new ResModel<SyncRun>("SyncRuleId обязателен.");
            if (!Enum.IsDefined(typeof(SyncRunTrigger), trigger))
                return new ResModel<SyncRun>("Некорректный Trigger.");

            return new ResModel<SyncRun>(new SyncRun
            {
                Id = Guid.NewGuid(),
                SyncRuleId = syncRuleId,
                StartedUtc = DateTime.UtcNow,
                Status = SyncRunStatus.Running,
                Trigger = trigger
            });
        }

        public static SyncRun Restore(
           Guid id,
           Guid syncRuleId,
           DateTime startedUtc,
           DateTime? finishedUtc,
           SyncRunStatus status,
           SyncRunTrigger trigger,
           int filesTotal,
           int filesSucceeded,
           int filesFailed,
           long bytesUploaded,
           string? errorMessage)
        {
            return new SyncRun
            {
                Id = id,
                SyncRuleId = syncRuleId,
                StartedUtc = startedUtc,
                FinishedUtc = finishedUtc,
                Status = status,
                Trigger = trigger,
                FilesTotal = filesTotal,
                FilesSucceeded = filesSucceeded,
                FilesFailed = filesFailed,
                BytesUploaded = bytesUploaded,
                ErrorMessage = errorMessage
            };
        }
    }
}
