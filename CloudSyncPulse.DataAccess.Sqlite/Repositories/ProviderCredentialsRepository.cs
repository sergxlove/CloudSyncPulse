using CloudSyncPulse.Core.Models;
using CloudSyncPulse.DataAccess.Sqlite.Abstractions;
using CloudSyncPulse.DataAccess.Sqlite.Infrastructures;
using Microsoft.EntityFrameworkCore;

namespace CloudSyncPulse.DataAccess.Sqlite.Repositories
{
    public class ProviderCredentialsRepository : IProviderCredentialsRepository
    {
        private readonly CloudSyncDbContext _db;

        public ProviderCredentialsRepository(CloudSyncDbContext db) => _db = db;

        public async Task<ResDb<ProviderCredential?>> GetByProviderIdAsync(Guid providerId, CancellationToken ct = default)
        {
            try
            {
                var e = await _db.ProviderCredentialsTable.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ProviderId == providerId, ct);
                return ResDb<ProviderCredential?>.Ok(e?.ToModel());
            }
            catch (Exception ex)
            {
                return ResDb<ProviderCredential?>.Fail(ex.Message);
            }
        }

        public async Task<ResDb> AddAsync(ProviderCredential credential, CancellationToken ct = default)
        {
            try
            {
                await _db.ProviderCredentialsTable.AddAsync(credential.ToEntity(), ct);
                await _db.SaveChangesAsync(ct);
                return ResDb.Ok();
            }
            catch (Exception ex)
            {
                return ResDb.Fail(ex.Message);
            }
        }

        public async Task<ResDb> UpdateAsync(ProviderCredential credential, CancellationToken ct = default)
        {
            try
            {
                var e = await _db.ProviderCredentialsTable
                    .FirstOrDefaultAsync(x => x.ProviderId == credential.ProviderId, ct);

                if (e is null)
                    return ResDb.Fail($"Креды для провайдера {credential.ProviderId} не найдены.");

                e.AccessTokenEncrypted = credential.AccessTokenEncrypted;
                e.RefreshTokenEncrypted = credential.RefreshTokenEncrypted;
                e.ExpiresAtUtc = credential.ExpiresAtUtc;
                e.Scope = credential.Scope;
                e.TokenType = credential.TokenType;
                e.UpdatedUtc = credential.UpdatedUtc;

                await _db.SaveChangesAsync(ct);
                return ResDb.Ok();
            }
            catch (Exception ex)
            {
                return ResDb.Fail(ex.Message);
            }
        }

        public async Task<ResDb> DeleteAsync(Guid providerId, CancellationToken ct = default)
        {
            try
            {
                var e = await _db.ProviderCredentialsTable
                    .FirstOrDefaultAsync(x => x.ProviderId == providerId, ct);
                if (e is null)
                    return ResDb.Ok();   // идемпотентно: нечего удалять — считаем успехом

                _db.ProviderCredentialsTable.Remove(e);
                await _db.SaveChangesAsync(ct);
                return ResDb.Ok();
            }
            catch (Exception ex)
            {
                return ResDb.Fail(ex.Message);
            }
        }
    }
}