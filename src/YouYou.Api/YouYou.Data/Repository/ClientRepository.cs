using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using YouYou.Business.Interfaces.Clients;
using YouYou.Business.Interfaces.Employees;
using YouYou.Business.Models;
using YouYou.Business.Models.Pagination;
using YouYou.Data.Context;

namespace YouYou.Data.Repository
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(YouYouContext db) : base(db)
        {
        }
        public async Task<IEnumerable<Client>> GetAllWithIncludes(ClientFilter filter)
        {
            return await Db.Clients.AsNoTracking().OrderBy(o => o.User.IsCompany ? o.User.JuridicalPerson.CompanyName : o.User.PhysicalPerson.Name)
                .Include(c => c.User)
                    .ThenInclude(c => c.PhysicalPerson)
                        .ThenInclude(p => p.Gender)
                            .ThenInclude(g => g.TypeGender)
                .Include(c => c.User)
                    .ThenInclude(c => c.JuridicalPerson)
                .Include(c => c.Address)
                    .ThenInclude(c => c.City)
                .Include(d => d.User)
                    .ThenInclude(u => u.UserRoles)
                        .ThenInclude(u => u.Role)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalRecords(ClientFilter filter)
        {
            return await Db.Clients.CountAsync();
        }
        public async Task<Client> GetByIdWithIncludes(Guid id)
        {
            return await Db.Clients.AsNoTracking()
                .Include(d => d.User)
                    .ThenInclude(u => u.PhysicalPerson)
                        .ThenInclude(d => d.Gender)
                            .ThenInclude(g => g.TypeGender)
                .Include(d => d.User)
                    .ThenInclude(u => u.JuridicalPerson)
                .Include(dd => dd.User)
                    .ThenInclude(d => d.ExtraPhones)
                .Include(d => d.Address)
                    .ThenInclude(a => a.City)
                .Include(d => d.User)
                    .ThenInclude(u => u.UserRoles)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<Client> GetByIdWithIncludesTracked(Guid id)
        {
            return await Db.Clients.AsTracking()
                .Include(d => d.User)
                    .ThenInclude(u => u.PhysicalPerson)
                        .ThenInclude(d => d.Gender)
                            .ThenInclude(g => g.TypeGender)
                .Include(dd => dd.User)
                    .ThenInclude(d => d.ExtraPhones)
                .Include(d => d.User)
                    .ThenInclude(u => u.JuridicalPerson)
                .Include(d => d.User)
                    .ThenInclude(u => u.UserRoles)
                .Include(d => d.Address)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
