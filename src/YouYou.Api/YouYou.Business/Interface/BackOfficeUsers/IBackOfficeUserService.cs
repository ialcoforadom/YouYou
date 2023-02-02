using Microsoft.AspNetCore.Http;
using YouYou.Business.Models;
using YouYou.Business.Models.Pagination;

namespace YouYou.Business.Interfaces.BackOfficeUsers
{
    public interface IBackOfficeUserService : IDisposable
    {
        Task Add(BackOfficeUser backOfficeUser, string password, Guid roleId, IFormFile file);
        Task<IEnumerable<BackOfficeUser>> GetAllWithIncludes(BackOfficeUsersFilter filter);
        Task<int> GetTotalRecords(BackOfficeUsersFilter filter);
        Task<BackOfficeUser> GetByIdWithIncludes(Guid id); 
        Task<BackOfficeUser> GetByIdWithIncludesTracked(Guid id);
        Task Update(BackOfficeUser backOfficeUser, string password, Guid roleId, IFormFile file);
        Task Remove(Guid id);
        Task<BackOfficeUser> GetById(Guid id);
        Task Disable(Guid id); 
        Task Enable(Guid id);
    }
}
