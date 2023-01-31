namespace YouYou.Api.ViewModels.Users
{
    public class UserTokenViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool ReadTermsOfUse { get; set; }

        public IEnumerable<ClaimViewModel> Claims { get; set; }
    }
}
