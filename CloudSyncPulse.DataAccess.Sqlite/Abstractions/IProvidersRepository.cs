using CloudSyncPulse.Core.Models;
using CloudSyncPulse.DataAccess.Sqlite.Infrastructures;

namespace CloudSyncPulse.DataAccess.Sqlite.Abstractions
{
    public interface IProvidersRepository
    {
        Task<ResDb> AddAsync(Providers provider, CancellationToken ct = default);
        Task<ResDb> DeleteAsync(Guid id, CancellationToken ct = default);
        Task<ResDb<IReadOnlyList<Providers>>> GetAllAsync(CancellationToken ct = default);
        Task<ResDb<Providers?>> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<ResDb<IReadOnlyList<Providers>>> GetEnabledAsync(CancellationToken ct = default);
        Task<ResDb> UpdateAsync(Providers provider, CancellationToken ct = default);
    }
}