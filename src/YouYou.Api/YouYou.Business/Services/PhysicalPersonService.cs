using YouYou.Business.Interfaces;
using YouYou.Business.Interfaces.PhysicalPersons;
using YouYou.Business.Models;
using YouYou.Business.Models.Validations;

namespace YouYou.Business.Services
{
    public class PhysicalPersonService : BaseService, IPhysicalPersonService
    {
        private readonly IPhysicalPersonRepository _physicalPersonRepository;

        public PhysicalPersonService(IErrorNotifier errorNotifier,
            IPhysicalPersonRepository physicalPersonRepository) : base(errorNotifier)
        {
            _physicalPersonRepository = physicalPersonRepository;
        }

        //public async Task<bool> Add(PhysicalPerson physicalPerson)
        //{
        //    if (!ExecuteValidation(new PhysicalPersonValidation(), physicalPerson)) return false;

        //    await _physicalPersonRepository.Add(physicalPerson);

        //    return true;
        //}

        //public async Task Update(PhysicalPerson physicalPerson)
        //{
        //    if (!ExecuteValidation(new PhysicalPersonValidation(), physicalPerson)) return;

        //    await _physicalPersonRepository.Update(physicalPerson);
        //}

        public void Dispose()
        {
            _physicalPersonRepository?.Dispose();
        }
    }
}
