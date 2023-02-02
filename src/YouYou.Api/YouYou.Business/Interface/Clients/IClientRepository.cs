using YouYou.Business.Models;
using YouYou.Business.Models.Pagination;

namespace YouYou.Business.Interfaces.Clients
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<IEnumerable<Client>> GetAllWithIncludes(ClientFilter filter);
        Task<int> GetTotalRecords(ClientFilter filter);
        Task<Client> GetByIdWithIncludes(Guid id);
        Task<Client> GetByIdWithIncludesTracked(Guid id);

    }
}
