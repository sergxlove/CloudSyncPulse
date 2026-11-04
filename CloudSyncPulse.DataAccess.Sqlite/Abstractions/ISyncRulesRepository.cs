using CloudSyncPulse.Core.Models;
using CloudSyncPulse.DataAccess.Sqlite.Infrastructures;

namespace CloudSyncPulse.DataAccess.Sqlite.Abstractions
{
    public interface ISyncRulesRepository
    {
        Task<ResDb> AddAsync(SyncRules rule, CancellationToken ct = default);
        Task<ResDb> DeleteAsync(Guid id, CancellationToken ct = default);
        Task<ResDb<IReadOnlyList<SyncRules>>> GetActiveAsync(CancellationToken ct = default);
        Task<ResDb<IReadOnlyList<SyncRules>>> GetAllAsync(bool includeDeleted = false, CancellationToken ct = default);
        Task<ResDb<SyncRules?>> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<ResDb<IReadOnlyList<SyncRules>>> GetDueAsync(DateTime nowUtc, CancellationToken ct = default);
        Task<ResDb> UpdateAsync(SyncRules rule, CancellationToken ct = default);
    }
}