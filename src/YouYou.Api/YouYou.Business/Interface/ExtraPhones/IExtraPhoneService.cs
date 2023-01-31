using YouYou.Business.Models;

namespace YouYou.Business.Interfaces.ExtraPhones
{
    public interface IExtraPhoneService : IDisposable
    {
        void AddPhones(ApplicationUser user, IList<string> phones);

        Task UpdatePhones(ApplicationUser user, IList<string> phones);

        ICollection<string> MapperPhones(ApplicationUser user);
        Task Remove(ApplicationUser user);
    }
}
