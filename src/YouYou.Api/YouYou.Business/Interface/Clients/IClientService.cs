using YouYou.Business.Models;

namespace YouYou.Business.Interfaces.Clients
{
    public interface IClientService
    {
        Task<bool> Add(Client client, string password);
    }
}
