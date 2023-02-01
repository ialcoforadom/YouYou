using YouYou.Business.Interfaces;
using YouYou.Business.Interfaces.BankDatas;
using YouYou.Business.Models;

namespace YouYou.Business.Services
{
    public class BankDataService : BaseService, IBankDataService
    {
        public BankDataService(IErrorNotifier errorNotifier) : base(errorNotifier)
        {
        }

        public void SetCpfOrCnpjHolderInBankData(BankData bankData, string CpfOrCnpjHolder)
        {
            if (bankData.IsHolder && string.IsNullOrEmpty(bankData.CpfOrCnpjHolder))
            {
                bankData.CpfOrCnpjHolder = CpfOrCnpjHolder;
            }
        }
    }
}
