using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using YouYou.Business.Interfaces.BackOfficeUsers;
using YouYou.Business.Models;
using YouYou.Business.Models.Pagination;
using YouYou.Data.Context;

namespace YouYou.Data.Repository
{
    public class BackOfficeUserRepository : Repository<BackOfficeUser>, IBackOfficeUserRepository
    {
        public BackOfficeUserRepository(YouYouContext db) : base(db)
        {
        }

        public async Task<IEnumerable<BackOfficeUser>> GetAllWithIncludes(BackOfficeUsersFilter filter)
        {
            return await Db.BackOfficeUsers.AsNoTracking().OrderBy(o => o.User.PhysicalPerson.Name)
                .Include(c => c.User)
                    .ThenInclude(c => c.PhysicalPerson)
                .Include(c => c.User)
                    .ThenInclude(c => c.UserRoles)
                        .ThenInclude(c => c.Role)
                .Where(ExpressionFilter(filter))
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalRecords(BackOfficeUsersFilter filter)
        {
            return await Db.BackOfficeUsers.Where(ExpressionFilter(filter)).CountAsync();
        }

        private Expression<Func<BackOfficeUser, bool>> ExpressionFilter(BackOfficeUsersFilter filter)
        {
            return c => c.User.PhysicalPerson.Name.Contains(filter.Name ?? string.Empty) &&
                        c.User.PhysicalPerson.CPF.Contains(filter.CPF ?? string.Empty) &&
                        c.User.Email.Contains(filter.Email ?? string.Empty) &&
                        c.User.UserRoles.FirstOrDefault().Role.Name.Contains(filter.Role ?? string.Empty);
        }

        public async Task<BackOfficeUser> GetByIdWithIncludes(Guid id)
        {
            return await Db.BackOfficeUsers.AsNoTracking()
                .Include(c => c.User)
                    .ThenInclude(c => c.PhysicalPerson)
                .Include(c => c.User)
                    .ThenInclude(c => c.UserRoles)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<BackOfficeUser> GetByIdWithIncludesTracked(Guid id)
        {
            return await Db.BackOfficeUsers.AsTracking()
                .Include(c => c.User)
                    .ThenInclude(c => c.PhysicalPerson)
                .Include(c => c.User)
                    .ThenInclude(c => c.UserRoles)
                        .ThenInclude(c => c.Role)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        //public override async Task Remove(Guid id)
        //{
        //    var backOfficeUser = await Db.BackOfficeUsers.AsTracking()
        //        .Include(c => c.User).FirstAsync(c => c.Id == id);

        //    backOfficeUser.IsDeleted = true;
        //    backOfficeUser.User.IsDeleted = true;

        //    await Update(backOfficeUser);
        //}
    }
}
