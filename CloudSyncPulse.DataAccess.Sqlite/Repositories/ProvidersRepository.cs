using CloudSyncPulse.Core.Models;
using CloudSyncPulse.DataAccess.Sqlite.Abstractions;
using CloudSyncPulse.DataAccess.Sqlite.Infrastructures;
using Microsoft.EntityFrameworkCore;

namespace CloudSyncPulse.DataAccess.Sqlite.Repositories
{
    public class ProvidersRepository : IProvidersRepository
    {
        private readonly CloudSyncDbContext _db;

        public ProvidersRepository(CloudSyncDbContext db) => _db = db;

        public async Task<ResDb<Providers?>> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            try
            {
                var e = await _db.ProvidersTable.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
                return ResDb<Providers?>.Ok(e?.ToModel());
            }
            catch (Exception ex)
            {
                return ResDb<Providers?>.Fail(ex.Message);
            }
        }

        public async Task<ResDb<IReadOnlyList<Providers>>> GetAllAsync(CancellationToken ct = default)
        {
            try
            {
                var list = await _db.ProvidersTable.AsNoTracking().ToListAsync(ct);
                return ResDb<IReadOnlyList<Providers>>.Ok(list.Select(x => x.ToModel()).ToList());
            }
            catch (Exception ex)
            {
                return ResDb<IReadOnlyList<Providers>>.Fail(ex.Message);
            }
        }

        public async Task<ResDb<IReadOnlyList<Providers>>> GetEnabledAsync(CancellationToken ct = default)
        {
            try
            {
                var list = await _db.ProvidersTable.AsNoTracking()
                    .Where(x => x.IsEnabled)
                    .ToListAsync(ct);
                return ResDb<IReadOnlyList<Providers>>.Ok(list.Select(x => x.ToModel()).ToList());
            }
            catch (Exception ex)
            {
                return ResDb<IReadOnlyList<Providers>>.Fail(ex.Message);
            }
        }

        public async Task<ResDb> AddAsync(Providers provider, CancellationToken ct = default)
        {
            try
            {
                await _db.ProvidersTable.AddAsync(provider.ToEntity(), ct);
                await _db.SaveChangesAsync(ct);
                return ResDb.Ok();
            }
            catch (Exception ex)
            {
                return ResDb.Fail(ex.Message);
            }
        }

        public async Task<ResDb> UpdateAsync(Providers provider, CancellationToken ct = default)
        {
            try
            {
                var e = await _db.ProvidersTable.FirstOrDefaultAsync(x => x.Id == provider.Id, ct);
                if (e is null)
                    return ResDb.Fail($"Провайдер {provider.Id} не найден.");

                e.Type = provider.Type;
                e.DisplayName = provider.DisplayName;
                e.IsEnabled = provider.IsEnabled;
                e.UpdatedUtc = provider.UpdatedUtc;

                await _db.SaveChangesAsync(ct);
                return ResDb.Ok();
            }
            catch (Exception ex)
            {
                return ResDb.Fail(ex.Message);
            }
        }

        public async Task<ResDb> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            try
            {
                var e = await _db.ProvidersTable.FirstOrDefaultAsync(x => x.Id == id, ct);
                if (e is null)
                    return ResDb.Ok();

                _db.ProvidersTable.Remove(e);
                await _db.SaveChangesAsync(ct);
                return ResDb.Ok();
            }
            catch (DbUpdateException ex)
            {
                return ResDb.Fail($"Не удалось удалить провайдера: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return ResDb.Fail(ex.Message);
            }
        }
    }
}