using YouYou.Business.Interfaces.PhysicalPersons;
using YouYou.Business.Models;
using YouYou.Data.Context;

namespace YouYou.Data.Repository
{
    public class PhysicalPersonRepository : Repository<PhysicalPerson>, IPhysicalPersonRepository
    {
        public PhysicalPersonRepository(YouYouContext db) : base(db)
        {
        }
    }
}
