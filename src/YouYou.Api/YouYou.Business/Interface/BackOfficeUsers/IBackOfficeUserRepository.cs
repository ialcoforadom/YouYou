using YouYou.Business.Models;
using YouYou.Business.Models.Pagination;

namespace YouYou.Business.Interfaces.BackOfficeUsers
{
    public interface IBackOfficeUserRepository : IRepository<BackOfficeUser>
    {
        Task<IEnumerable<BackOfficeUser>> GetAllWithIncludes(BackOfficeUsersFilter filter);
        Task<int> GetTotalRecords(BackOfficeUsersFilter filter); 
        Task<BackOfficeUser> GetByIdWithIncludes(Guid id);
        Task<BackOfficeUser> GetByIdWithIncludesTracked(Guid id);
    }
}
