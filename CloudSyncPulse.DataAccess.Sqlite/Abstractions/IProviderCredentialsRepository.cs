using CloudSyncPulse.Core.Models;
using CloudSyncPulse.DataAccess.Sqlite.Infrastructures;

namespace CloudSyncPulse.DataAccess.Sqlite.Abstractions
{
    public interface IProviderCredentialsRepository
    {
        Task<ResDb> AddAsync(ProviderCredential credential, CancellationToken ct = default);
        Task<ResDb> DeleteAsync(Guid providerId, CancellationToken ct = default);
        Task<ResDb<ProviderCredential?>> GetByProviderIdAsync(Guid providerId, CancellationToken ct = default);
        Task<ResDb> UpdateAsync(ProviderCredential credential, CancellationToken ct = default);
    }
}