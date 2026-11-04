using CloudSyncPulse.Core.Models;
using CloudSyncPulse.DataAccess.Sqlite.Models;

namespace CloudSyncPulse.DataAccess.Sqlite.Infrastructures
{
    public static class EntityMapper
    {
        public static Users ToModel(this UsersEntity e) => Users.Restore(
           e.Id, e.Username, e.PasswordHash);

        public static UsersEntity ToEntity(this Users m) => new()
        {
            Id = m.Id,
            Username = m.Username,
            PasswordHash = m.PasswordHash
        };

        public static Providers ToModel(this ProvidersEntity e) => Providers.Restore(
            e.Id, e.Type, e.DisplayName, e.IsEnabled, e.CreatedUtc, e.UpdatedUtc);

        public static ProvidersEntity ToEntity(this Providers m) => new()
        {
            Id = m.Id,
            Type = m.Type,
            DisplayName = m.DisplayName,
            IsEnabled = m.IsEnabled,
            CreatedUtc = m.CreatedUtc,
            UpdatedUtc = m.UpdatedUtc
        };

        public static ProviderCredential ToModel(this ProviderCredentialEntity e) => ProviderCredential.Restore(
            e.ProviderId, e.AccessTokenEncrypted, e.RefreshTokenEncrypted,
            e.ExpiresAtUtc, e.Scope, e.TokenType, e.UpdatedUtc);

        public static ProviderCredentialEntity ToEntity(this ProviderCredential m) => new()
        {
            ProviderId = m.ProviderId,
            AccessTokenEncrypted = m.AccessTokenEncrypted,
            RefreshTokenEncrypted = m.RefreshTokenEncrypted,
            ExpiresAtUtc = m.ExpiresAtUtc,
            Scope = m.Scope,
            TokenType = m.TokenType,
            UpdatedUtc = m.UpdatedUtc
        };

        public static SyncRules ToModel(this SyncRulesEntity e) => SyncRules.Restore(
            e.Id, e.Name, e.Enabled, e.LocalPath, e.LocalPathType, e.Recursive,
            e.ProviderId, e.RemoteFolder, e.RemoteFileNameTemplate, e.OverwritePolicy,
            e.DeleteAfterUpload, e.Cron, e.LastRunUtc, e.NextRunUtc, e.IsDeleted);

        public static SyncRulesEntity ToEntity(this SyncRules m) => new()
        {
            Id = m.Id,
            Name = m.Name,
            Enabled = m.Enabled,
            LocalPath = m.LocalPath,
            LocalPathType = m.LocalPathType,
            Recursive = m.Recursive,
            ProviderId = m.ProviderId,
            RemoteFolder = m.RemoteFolder,
            RemoteFileNameTemplate = m.RemoteFileNameTemplate,
            OverwritePolicy = m.OverwritePolicy,
            DeleteAfterUpload = m.DeleteAfterUpload,
            Cron = m.Cron,
            LastRunUtc = m.LastRunUtc,
            NextRunUtc = m.NextRunUtc,
            IsDeleted = m.IsDeleted
        };

        public static SyncRun ToModel(this SyncRunEntity e) => SyncRun.Restore(
            e.Id, e.SyncRuleId, e.StartedUtc, e.FinishedUtc, e.Status, e.Trigger,
            e.FilesTotal, e.FilesSucceeded, e.FilesFailed, e.BytesUploaded, e.ErrorMessage);

        public static SyncRunEntity ToEntity(this SyncRun m) => new()
        {
            Id = m.Id,
            SyncRuleId = m.SyncRuleId,
            StartedUtc = m.StartedUtc,
            FinishedUtc = m.FinishedUtc,
            Status = m.Status,
            Trigger = m.Trigger,
            FilesTotal = m.FilesTotal,
            FilesSucceeded = m.FilesSucceeded,
            FilesFailed = m.FilesFailed,
            BytesUploaded = m.BytesUploaded,
            ErrorMessage = m.ErrorMessage
        };
    }
}
