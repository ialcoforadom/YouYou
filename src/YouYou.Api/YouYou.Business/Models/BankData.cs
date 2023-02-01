namespace YouYou.Business.Models
{
    public class BankData : Entity
    {
        public string BankName { get; set; }

        public string Agency { get; set; }

        public string Account { get; set; }

        public string CpfOrCnpjHolder { get; set; }

        public bool IsHolder { get; set; }

        public string PixKey { get; set; }
    }
}
