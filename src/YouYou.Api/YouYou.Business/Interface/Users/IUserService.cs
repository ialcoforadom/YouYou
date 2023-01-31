using YouYou.Business.Models;

namespace YouYou.Business.Interfaces.Users
{
    public interface IUserService
    {
        Task<ApplicationUser> GetByIdWithPerson(Guid id);

        Task<bool> Add(ApplicationUser user, string password);

        Task<bool> AddRoleEditor(ApplicationUser user);

        Task<bool> AddRolePhotography(ApplicationUser user);

        Task<bool> AddRoleCoordinator(ApplicationUser user);

        Task<bool> Update(ApplicationUser user);

        Task<bool> Update(ApplicationUser user, string password);

        Task<bool> UpdatePassword(Guid id, string password);

        Task<bool> UpdateRole(ApplicationUser user, Guid newRoleId);

        Task<bool> RemoveRole(ApplicationUser user, string role);

        Task<bool> AddRole(ApplicationUser user, string role);

        Task<bool> AddRole(ApplicationUser user, Guid roleId);

        ICollection<ApplicationRole> GetRoles();

        ICollection<ApplicationRole> GetRolesBackOfficeUsers();

        Task<bool> Remove(Guid id);

        Task RemoveWithoutLogicaDeletion(Guid id);

        Task Disable(Guid id);

        Task Enable(Guid id);
        //Task ConfirmTermsOfUse(TermsOfUseDto TermsOfUse);
    }
}
