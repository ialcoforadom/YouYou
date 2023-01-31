using YouYou.Business.Interfaces.ExtraPhones;
using YouYou.Business.Models;
using YouYou.Data.Context;

namespace YouYou.Data.Repository
{
    public class ExtraPhoneRepository : Repository<ExtraPhone>, IExtraPhoneRepository
    {
        public ExtraPhoneRepository(YouYouContext db) : base(db)
        {
        }
    }
}
