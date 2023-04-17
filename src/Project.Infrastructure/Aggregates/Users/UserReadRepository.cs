using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.Application.Aggregates.Users;
using Project.Application.Aggregates.Users.Queries;

namespace Project.Infrastructure.Aggregates.Users
{
    public class UserReadRepository : IUserReadRepository
    {
        private readonly ReadDbContext dbContext;

        public UserReadRepository(ReadDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<UserQueryResult> GetAll()
        {
            return dbContext.UserQueryResults.FromSqlRaw(@"
                    SELECT      U.""Id"",
                                U.""Username"",
                                U.""Address"",
                                U.""Description"",
                                COALESCE(UC.""Username"", U.""CreatorId""::TEXT) AS ""Creator"",
                                COALESCE(UU.""Username"", U.""UpdaterId""::TEXT) AS ""Updater"",
                                U.""Created"",
                                U.""Updated""
                    FROM        ""Users"" AS U
                    LEFT JOIN   ""Users""       AS UC   ON U.""CreatorId""      = UC.""Id""
                    LEFT JOIN   ""Users""       AS UU   ON U.""UpdaterId""      = UU.""Id""
                ");
        }

        public Task<UserQueryResult> GetByUsername(
            string username,
            CancellationToken cancellationToken = default)
        {
            return dbContext.UserQueryResults.FromSqlRaw(@"
                    SELECT      U.""Id"",
                                U.""Username"",
                                U.""Address"",
                                U.""Description"",
                                COALESCE(UC.""Username"", U.""CreatorId""::TEXT) AS ""Creator"",
                                COALESCE(UU.""Username"", U.""UpdaterId""::TEXT) AS ""Updater"",
                                U.""Created"",
                                U.""Updated""
                    FROM        ""Users"" AS U
                    LEFT JOIN   ""Users""       AS UC   ON U.""CreatorId""      = UC.""Id""
                    LEFT JOIN   ""Users""       AS UU   ON U.""UpdaterId""      = UU.""Id""
                    WHERE       U.""Username"" = {0}
                ", username).FirstOrDefaultAsync(cancellationToken);
        }
    }
}