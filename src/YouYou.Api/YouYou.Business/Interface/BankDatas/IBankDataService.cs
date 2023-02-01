using YouYou.Business.Models;

namespace YouYou.Business.Interfaces.BankDatas
{
    public interface IBankDataService
    {
        void SetCpfOrCnpjHolderInBankData(BankData bankData, string CpfOrCnpjHolder);
    }
}
