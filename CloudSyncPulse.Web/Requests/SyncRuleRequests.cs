using CloudSyncPulse.Core.Enums;

namespace CloudSyncPulse.Web.Requests
{
    public class CreateSyncRuleRequest
    {
        public string Name { get; set; } = string.Empty;
        public string LocalPath { get; set; } = string.Empty;
        public LocalPathType LocalPathType { get; set; } = LocalPathType.File;
        public Guid ProviderId { get; set; }
        public string Cron { get; set; } = "0 0 * * *";
        public string RemoteFolder { get; set; } = "/";
        public bool Enabled { get; set; } = true;
        public bool Recursive { get; set; }
        public string? RemoteFileNameTemplate { get; set; }
        public OverwritePolicy OverwritePolicy { get; set; } = OverwritePolicy.Skip;
        public bool DeleteAfterUpload { get; set; }
    }

    public class UpdateSyncRuleRequest : CreateSyncRuleRequest
    {
        public Guid Id { get; set; }
    }
}
