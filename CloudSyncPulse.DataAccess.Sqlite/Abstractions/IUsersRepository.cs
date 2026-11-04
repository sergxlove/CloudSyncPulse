using CloudSyncPulse.Core.Models;
using CloudSyncPulse.DataAccess.Sqlite.Infrastructures;

namespace CloudSyncPulse.DataAccess.Sqlite.Abstractions
{
    public interface IUsersRepository
    {
        Task<ResDb> AddAsync(Users user, CancellationToken ct = default);
        Task<ResDb> DeleteAsync(Guid id, CancellationToken ct = default);
        Task<ResDb<IReadOnlyList<Users>>> GetAllAsync(CancellationToken ct = default);
        Task<ResDb<Users?>> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<ResDb<Users?>> GetByUsernameAsync(string username, CancellationToken ct = default);
        Task<ResDb> UpdateAsync(Users user, CancellationToken ct = default);
    }
}