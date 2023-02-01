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
    }
}
