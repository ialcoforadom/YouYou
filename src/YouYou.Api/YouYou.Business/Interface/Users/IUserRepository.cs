using YouYou.Business.Models;

namespace YouYou.Business.Interfaces.Users
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetByIdWithPerson(Guid id);

        Task RemoveWithoutLogicaDeletion(ApplicationUser identityUser);
    }
}
