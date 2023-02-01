using YouYou.Business.Models.Dtos;
using YouYou.Business.Models.Enums;

namespace YouYou.Business.Interfaces.Clients
{
    public interface IClientService
    {
        Task Add(ClientDto clientDto, Guid roleId, TypeClientEnum type);
    }
}
