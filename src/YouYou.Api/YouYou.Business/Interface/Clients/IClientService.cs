using Microsoft.AspNetCore.Http;
using YouYou.Business.Models;
using YouYou.Business.Models.Dtos;
using YouYou.Business.Models.Enums;
using YouYou.Business.Models.Pagination;

namespace YouYou.Business.Interfaces.Clients
{
    public interface IClientService
    {
        Task Add(ClientDto clientDto, TypeClientEnum type, IFormFile file);
        Task<IEnumerable<Client>> GetAllWithIncludes(ClientFilter filter);
        Task<int> GetTotalRecords(ClientFilter filter); 
        Task<Client> GetById(Guid id);
        Task Remove(Client employee);
        Task<ClientDto> GetDtoByIdWithIncludes(Guid id); 
        Task Disable(Guid id);
        Task Enable(Guid id); 
        Task<Client> GetByIdWithIncludesTracked(Guid id);
        Task Update(ClientDto clientDto, TypeClientEnum type, IFormFile file);
    }
}
