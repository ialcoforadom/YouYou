namespace YouYou.Api.ViewModels.Clients
{
    public class ClientListViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string NickName { get; set; }

        public string City { get; set; }

        public DateTime? Birthday { get; set; }

        public string GenderName { get; set; }

        public ICollection<string> RolesNames { get; set; }

        public bool Disabled { get; set; }
    }
}
