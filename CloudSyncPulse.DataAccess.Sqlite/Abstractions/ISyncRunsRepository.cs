using CloudSyncPulse.Core.Models;
using CloudSyncPulse.DataAccess.Sqlite.Infrastructures;

namespace CloudSyncPulse.DataAccess.Sqlite.Abstractions
{
    public interface ISyncRunsRepository
    {
        Task<ResDb> AddAsync(SyncRun run, CancellationToken ct = default);
        Task<ResDb<SyncRun?>> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<ResDb<IReadOnlyList<SyncRun>>> GetByRuleIdAsync(Guid ruleId, int take = 50, CancellationToken ct = default);
        Task<ResDb<IReadOnlyList<SyncRun>>> GetRunningAsync(CancellationToken ct = default);
        Task<ResDb> UpdateAsync(SyncRun run, CancellationToken ct = default);
    }
}