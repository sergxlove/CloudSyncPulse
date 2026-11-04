using CloudSyncPulse.Core.Enums;
using CloudSyncPulse.Core.Models;
using CloudSyncPulse.DataAccess.Sqlite.Abstractions;
using CloudSyncPulse.DataAccess.Sqlite.Infrastructures;
using Microsoft.EntityFrameworkCore;

namespace CloudSyncPulse.DataAccess.Sqlite.Repositories
{
    public class SyncRunsRepository : ISyncRunsRepository
    {
        private readonly CloudSyncDbContext _db;

        public SyncRunsRepository(CloudSyncDbContext db) => _db = db;

        public async Task<ResDb<SyncRun?>> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            try
            {
                var e = await _db.SyncRunTable.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
                return ResDb<SyncRun?>.Ok(e?.ToModel());
            }
            catch (Exception ex)
            {
                return ResDb<SyncRun?>.Fail(ex.Message);
            }
        }

        public async Task<ResDb<IReadOnlyList<SyncRun>>> GetByRuleIdAsync(Guid ruleId, int take = 50, CancellationToken ct = default)
        {
            try
            {
                var list = await _db.SyncRunTable.AsNoTracking()
                    .Where(x => x.SyncRuleId == ruleId)
                    .OrderByDescending(x => x.StartedUtc)
                    .Take(take)
                    .ToListAsync(ct);
                return ResDb<IReadOnlyList<SyncRun>>.Ok(list.Select(x => x.ToModel()).ToList());
            }
            catch (Exception ex)
            {
                return ResDb<IReadOnlyList<SyncRun>>.Fail(ex.Message);
            }
        }

        public async Task<ResDb<IReadOnlyList<SyncRun>>> GetRunningAsync(CancellationToken ct = default)
        {
            try
            {
                var list = await _db.SyncRunTable.AsNoTracking()
                    .Where(x => x.Status == SyncRunStatus.Running)
                    .ToListAsync(ct);
                return ResDb<IReadOnlyList<SyncRun>>.Ok(list.Select(x => x.ToModel()).ToList());
            }
            catch (Exception ex)
            {
                return ResDb<IReadOnlyList<SyncRun>>.Fail(ex.Message);
            }
        }

        public async Task<ResDb> AddAsync(SyncRun run, CancellationToken ct = default)
        {
            try
            {
                await _db.SyncRunTable.AddAsync(run.ToEntity(), ct);
                await _db.SaveChangesAsync(ct);
                return ResDb.Ok();
            }
            catch (Exception ex)
            {
                return ResDb.Fail(ex.Message);
            }
        }

        public async Task<ResDb> UpdateAsync(SyncRun run, CancellationToken ct = default)
        {
            try
            {
                var e = await _db.SyncRunTable.FirstOrDefaultAsync(x => x.Id == run.Id, ct);
                if (e is null)
                    return ResDb.Fail($"Запуск {run.Id} не найден.");

                e.FinishedUtc = run.FinishedUtc;
                e.Status = run.Status;
                e.FilesTotal = run.FilesTotal;
                e.FilesSucceeded = run.FilesSucceeded;
                e.FilesFailed = run.FilesFailed;
                e.BytesUploaded = run.BytesUploaded;
                e.ErrorMessage = run.ErrorMessage;

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