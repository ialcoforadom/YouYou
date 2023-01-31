using System.Transactions;
using YouYou.Business.Interfaces;
using YouYou.Business.Interfaces.ExtraPhones;
using YouYou.Business.Models;

namespace YouYou.Business.Services
{
    public class ExtraPhoneService : BaseService, IExtraPhoneService
    {
        private readonly IExtraPhoneRepository _extraPhoneRepository;

        public ExtraPhoneService(IErrorNotifier errorNotifier, 
            IExtraPhoneRepository extraPhoneRepository) : base(errorNotifier)
        {
            _extraPhoneRepository = extraPhoneRepository;
        }

        public void AddPhones(ApplicationUser user, IList<string> phones)
        {
            if (phones.Count > 0)
            {
                user.PhoneNumber = phones[0];
                phones.Remove(phones[0]);

                user.ExtraPhones = new List<ExtraPhone>();

                foreach (string phone in phones)
                {
                    user.ExtraPhones.Add(new ExtraPhone() { Number = phone });
                }
            }
        }

        public async Task UpdatePhones(ApplicationUser user, IList<string> phones)
        {
            if (phones.Count > 0)
            {
                user.PhoneNumber = phones[0];
                phones.Remove(phones[0]);

                if (user.ExtraPhones != null && user.ExtraPhones.Count > 0)
                {
                    await _extraPhoneRepository.RemoveRange(user.ExtraPhones);
                }

                var extraPhones = new List<ExtraPhone>();

                foreach (string phone in phones)
                {
                    ExtraPhone extraPhone = new ExtraPhone(phone, user.Id);
                    extraPhones.Add(extraPhone);
                }

                await _extraPhoneRepository.AddRange(extraPhones);
            }
        }

        public ICollection<string> MapperPhones(ApplicationUser user)
        {
            List<string> phones = new List<string>();
            phones.Add(user.PhoneNumber);
            phones.AddRange(user.ExtraPhones.Select(c => c.Number));

            return phones;
        }

        public async Task Remove(ApplicationUser user)
        {
            using (TransactionScope tr = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {  
                await _extraPhoneRepository.RemoveRange(user.ExtraPhones);   
                tr.Complete();
            }
        }

        public void Dispose()
        {
            _extraPhoneRepository?.Dispose();
        }
    }
}
