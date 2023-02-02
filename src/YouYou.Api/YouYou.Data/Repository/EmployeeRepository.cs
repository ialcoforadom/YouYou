using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using YouYou.Business.Interfaces.Employees;
using YouYou.Business.Models;
using YouYou.Business.Models.Pagination;
using YouYou.Data.Context;

namespace YouYou.Data.Repository
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(YouYouContext db) : base(db)
        {
        }
        public async Task<IEnumerable<Employee>> GetAllWithIncludes(EmployeeFilter filter)
        {
            return await Db.Employees.AsNoTracking().OrderBy(o => o.User.PhysicalPerson.Name)
                    .Include(d => d.User)
                        .ThenInclude(u => u.PhysicalPerson)
                            .ThenInclude(e => e.Gender)
                                .ThenInclude(g => g.TypeGender)
                    .Include(d => d.User)
                        .ThenInclude(u => u.UserRoles)
                            .ThenInclude(r => r.Role)
                    .Include(d => d.Address)
                        .ThenInclude(a => a.City)
                .Where(ExpressionFilter(filter))
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
        }
        public async Task<int> GetTotalRecords(EmployeeFilter filter)
        {
            return await Db.Employees.Where(ExpressionFilter(filter)).CountAsync();
        }
        private Expression<Func<Employee, bool>> ExpressionFilter(EmployeeFilter filter)
        {
            if (string.IsNullOrEmpty(filter.NickName))
            {
                return dd => dd.User.PhysicalPerson.Name.Contains(filter.Name ?? string.Empty) &&
                        dd.Address.City.Name.Contains(filter.City ?? string.Empty) &&
                        dd.User.IsDeleted == false;

            }
            return dd => dd.User.PhysicalPerson.Name.Contains(filter.Name ?? string.Empty) &&
                        dd.User.NickName.Contains(filter.NickName) &&
                        dd.Address.City.Name.Contains(filter.City ?? string.Empty) &&
                        dd.User.IsDeleted == false;
        }
        public async Task<Employee> GetByIdWithIncludes(Guid id)
        {
            return await Db.Employees.AsNoTracking()
                .Include(d => d.User)
                    .ThenInclude(u => u.PhysicalPerson)
                        .ThenInclude(d => d.Gender)
                            .ThenInclude(g => g.TypeGender)
                .Include(dd => dd.User)
                    .ThenInclude(d => d.ExtraPhones)
                .Include(dd => dd.User)
                    .ThenInclude(d => d.UserRoles)
                .Include(d => d.Address)
                    .ThenInclude(a => a.City)
                .Include(dd => dd.BankData)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<Employee> GetByIdWithIncludesTracked(Guid id)
        {
            return await Db.Employees.AsTracking()
                .Include(d => d.User)
                    .ThenInclude(u => u.PhysicalPerson)
                        .ThenInclude(d => d.Gender)
                            .ThenInclude(g => g.TypeGender)
                .Include(dd => dd.User)
                    .ThenInclude(d => d.ExtraPhones)
                .Include(dd => dd.User)
                    .ThenInclude(d => d.UserRoles)
                .Include(d => d.Address)
                .Include(dd => dd.BankData)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
