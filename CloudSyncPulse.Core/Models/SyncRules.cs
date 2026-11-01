using CloudSyncPulse.Core.Enums;
using CloudSyncPulse.Core.Infrastructures;

namespace CloudSyncPulse.Core.Models
{
    public class SyncRules
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public bool Enabled { get; private set; }
        public string LocalPath { get; private set; } = string.Empty;
        public LocalPathType LocalPathType { get; private set; }
        public bool Recursive { get; private set; }
        public Guid ProviderId { get; private set; }
        public string RemoteFolder { get; private set; } = "/";
        public string? RemoteFileNameTemplate { get; private set; }
        public OverwritePolicy OverwritePolicy { get; private set; }
        public bool DeleteAfterUpload { get; private set; }
        public string Cron { get; private set; } = "0 0 * * *";
        public DateTime? LastRunUtc { get; private set; }
        public DateTime? NextRunUtc { get; private set; }
        public bool IsDeleted { get; private set; }

        private SyncRules() { }

        public static ResModel<SyncRules> Create(
            string name,
            string localPath,
            LocalPathType localPathType,
            Guid providerId,
            string cron,
            string remoteFolder = "/",
            bool enabled = true,
            bool recursive = false,
            string? remoteFileNameTemplate = null,
            OverwritePolicy overwritePolicy = OverwritePolicy.Skip,
            bool deleteAfterUpload = false)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new ResModel<SyncRules>("Name обязателен.");
            if (name.Length > 200)
                return new ResModel<SyncRules>("Name не должен превышать 200 символов.");

            if (string.IsNullOrWhiteSpace(localPath))
                return new ResModel<SyncRules>("LocalPath обязателен.");

            if (!Enum.IsDefined(typeof(LocalPathType), localPathType))
                return new ResModel<SyncRules>("Некорректный LocalPathType.");

            if (providerId == Guid.Empty)
                return new ResModel<SyncRules>("ProviderId обязателен.");

            if (string.IsNullOrWhiteSpace(cron))
                return new ResModel<SyncRules>("Cron обязателен.");

            if (!Enum.IsDefined(typeof(OverwritePolicy), overwritePolicy))
                return new ResModel<SyncRules>("Некорректный OverwritePolicy.");

            return new ResModel<SyncRules>(new SyncRules
            {
                Id = Guid.NewGuid(),
                Name = name.Trim(),
                Enabled = enabled,
                LocalPath = localPath,
                LocalPathType = localPathType,
                Recursive = recursive,
                ProviderId = providerId,
                RemoteFolder = string.IsNullOrWhiteSpace(remoteFolder) ? "/" : remoteFolder,
                RemoteFileNameTemplate = remoteFileNameTemplate,
                OverwritePolicy = overwritePolicy,
                DeleteAfterUpload = deleteAfterUpload,
                Cron = cron,
                IsDeleted = false
            });
        }
    }
}
