using CloudSyncPulse.Core.Models;
using CloudSyncPulse.DataAccess.Sqlite.Abstractions;
using CloudSyncPulse.DataAccess.Sqlite.Infrastructures;
using Microsoft.EntityFrameworkCore;

namespace CloudSyncPulse.DataAccess.Sqlite.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly CloudSyncDbContext _db;

        public UsersRepository(CloudSyncDbContext db) => _db = db;

        public async Task<ResDb<Users?>> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            try
            {
                var e = await _db.UsersTable.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
                return ResDb<Users?>.Ok(e?.ToModel());
            }
            catch (Exception ex)
            {
                return ResDb<Users?>.Fail(ex.Message);
            }
        }

        public async Task<ResDb<Users?>> GetByUsernameAsync(string username, CancellationToken ct = default)
        {
            try
            {
                var e = await _db.UsersTable.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Username == username, ct);
                return ResDb<Users?>.Ok(e?.ToModel());
            }
            catch (Exception ex)
            {
                return ResDb<Users?>.Fail(ex.Message);
            }
        }

        public async Task<ResDb<IReadOnlyList<Users>>> GetAllAsync(CancellationToken ct = default)
        {
            try
            {
                var list = await _db.UsersTable.AsNoTracking().ToListAsync(ct);
                return ResDb<IReadOnlyList<Users>>.Ok(list.Select(x => x.ToModel()).ToList());
            }
            catch (Exception ex)
            {
                return ResDb<IReadOnlyList<Users>>.Fail(ex.Message);
            }
        }

        public async Task<ResDb> AddAsync(Users user, CancellationToken ct = default)
        {
            try
            {
                await _db.UsersTable.AddAsync(user.ToEntity(), ct);
                await _db.SaveChangesAsync(ct);
                return ResDb.Ok();
            }
            catch (Exception ex)
            {
                return ResDb.Fail(ex.Message);
            }
        }

        public async Task<ResDb> UpdateAsync(Users user, CancellationToken ct = default)
        {
            try
            {
                var e = await _db.UsersTable.FirstOrDefaultAsync(x => x.Id == user.Id, ct);
                if (e is null)
                    return ResDb.Fail($"Пользователь {user.Id} не найден.");

                e.Username = user.Username;
                e.PasswordHash = user.PasswordHash;

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
                var e = await _db.UsersTable.FirstOrDefaultAsync(x => x.Id == id, ct);
                if (e is null)
                    return ResDb.Ok();

                _db.UsersTable.Remove(e);
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