using CloudSyncPulse.Core.Models;
using CloudSyncPulse.DataAccess.Sqlite.Abstractions;
using CloudSyncPulse.DataAccess.Sqlite.Infrastructures;
using Microsoft.EntityFrameworkCore;

namespace CloudSyncPulse.DataAccess.Sqlite.Repositories
{
    public class SyncRulesRepository : ISyncRulesRepository
    {
        private readonly CloudSyncDbContext _db;

        public SyncRulesRepository(CloudSyncDbContext db) => _db = db;

        public async Task<ResDb<SyncRules?>> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            try
            {
                var e = await _db.SyncRulesTable.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
                return ResDb<SyncRules?>.Ok(e?.ToModel());
            }
            catch (Exception ex)
            {
                return ResDb<SyncRules?>.Fail(ex.Message);
            }
        }

        public async Task<ResDb<IReadOnlyList<SyncRules>>> GetAllAsync(bool includeDeleted = false, CancellationToken ct = default)
        {
            try
            {
                var q = _db.SyncRulesTable.AsNoTracking().AsQueryable();
                if (!includeDeleted) q = q.Where(x => !x.IsDeleted);

                var list = await q.ToListAsync(ct);
                return ResDb<IReadOnlyList<SyncRules>>.Ok(list.Select(x => x.ToModel()).ToList());
            }
            catch (Exception ex)
            {
                return ResDb<IReadOnlyList<SyncRules>>.Fail(ex.Message);
            }
        }

        public async Task<ResDb<IReadOnlyList<SyncRules>>> GetActiveAsync(CancellationToken ct = default)
        {
            try
            {
                var list = await _db.SyncRulesTable.AsNoTracking()
                    .Where(x => x.Enabled && !x.IsDeleted)
                    .ToListAsync(ct);
                return ResDb<IReadOnlyList<SyncRules>>.Ok(list.Select(x => x.ToModel()).ToList());
            }
            catch (Exception ex)
            {
                return ResDb<IReadOnlyList<SyncRules>>.Fail(ex.Message);
            }
        }

        public async Task<ResDb<IReadOnlyList<SyncRules>>> GetDueAsync(DateTime nowUtc, CancellationToken ct = default)
        {
            try
            {
                var list = await _db.SyncRulesTable.AsNoTracking()
                    .Where(x => x.Enabled
                                && !x.IsDeleted
                                && (x.NextRunUtc == null || x.NextRunUtc <= nowUtc))
                    .ToListAsync(ct);
                return ResDb<IReadOnlyList<SyncRules>>.Ok(list.Select(x => x.ToModel()).ToList());
            }
            catch (Exception ex)
            {
                return ResDb<IReadOnlyList<SyncRules>>.Fail(ex.Message);
            }
        }

        public async Task<ResDb> AddAsync(SyncRules rule, CancellationToken ct = default)
        {
            try
            {
                await _db.SyncRulesTable.AddAsync(rule.ToEntity(), ct);
                await _db.SaveChangesAsync(ct);
                return ResDb.Ok();
            }
            catch (Exception ex)
            {
                return ResDb.Fail(ex.Message);
            }
        }

        public async Task<ResDb> UpdateAsync(SyncRules rule, CancellationToken ct = default)
        {
            try
            {
                var e = await _db.SyncRulesTable.FirstOrDefaultAsync(x => x.Id == rule.Id, ct);
                if (e is null)
                    return ResDb.Fail($"Правило {rule.Id} не найдено.");

                e.Name = rule.Name;
                e.Enabled = rule.Enabled;
                e.LocalPath = rule.LocalPath;
                e.LocalPathType = rule.LocalPathType;
                e.Recursive = rule.Recursive;
                e.ProviderId = rule.ProviderId;
                e.RemoteFolder = rule.RemoteFolder;
                e.RemoteFileNameTemplate = rule.RemoteFileNameTemplate;
                e.OverwritePolicy = rule.OverwritePolicy;
                e.DeleteAfterUpload = rule.DeleteAfterUpload;
                e.Cron = rule.Cron;
                e.LastRunUtc = rule.LastRunUtc;
                e.NextRunUtc = rule.NextRunUtc;
                e.IsDeleted = rule.IsDeleted;

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
                var e = await _db.SyncRulesTable.FirstOrDefaultAsync(x => x.Id == id, ct);
                if (e is null)
                    return ResDb.Ok();

                e.IsDeleted = true;
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