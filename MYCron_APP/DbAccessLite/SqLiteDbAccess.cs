using Microsoft.EntityFrameworkCore;
using MYCentralModels.SQLite;
using MYCron_APP.DbContextLite;

namespace MYCron_APP.DbAccessLite
{
    public class SqLiteDbAccess(SqLiteDbContext dbContext)
    {
        private readonly SqLiteDbContext _dbContext = dbContext;
        public async Task SetUserToSession(UserSession userSession)
        {
            if (!_dbContext.UserSessions.Where(user => user.Id == userSession.Id).Any())
            {
                await _dbContext.UserSessions.AddAsync(userSession);
            }
            else
            {
                _dbContext.UserSessions.Update(userSession);
            }
            await _dbContext.SaveChangesAsync();
        }
        public async Task<UserSession?> GetUserToSession(int? Id)
        {
            UserSession? userSession = null;
            if (Id != null)
            {
                userSession = await _dbContext.UserSessions.Where(user => user.Id == Id).FirstOrDefaultAsync();
            }
            return userSession;
        }

        public async Task RemoveUserFromSession(int? Id)
        {
            if (Id == null) return;
            UserSession? userSession = _dbContext.UserSessions.Where(user => user.Id == Id).FirstOrDefault();
            if (userSession != null)
            {
                _dbContext.UserSessions.Remove(userSession);
            }
            await _dbContext.SaveChangesAsync();
        }
        public async Task GetOutOfSessionPages()
        {
            await _dbContext.OutOfSessionPages.ToListAsync();
        }
    }
}