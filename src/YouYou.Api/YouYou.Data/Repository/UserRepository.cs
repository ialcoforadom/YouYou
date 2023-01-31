using Microsoft.EntityFrameworkCore;
using YouYou.Business.Interfaces.Users;
using YouYou.Business.Models;
using YouYou.Data.Context;

namespace YouYou.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly YouYouContext Db;
        protected readonly DbSet<ApplicationUser> DbSet;

        public UserRepository(YouYouContext db)
        {
            Db = db;
            DbSet = db.Set<ApplicationUser>();
        }

        public async Task<ApplicationUser> GetByIdWithPerson(Guid id)
        {
            return await Db.ApplicationUsers.AsNoTracking()
                .Include(c => c.PhysicalPerson)
                .Include(c => c.JuridicalPerson)
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task RemoveWithoutLogicaDeletion(ApplicationUser applicationUser)
        {
            DbSet.Remove(applicationUser);
            await Db.SaveChangesWithoutLogicaDeletionAsync();
        }
    }
}
