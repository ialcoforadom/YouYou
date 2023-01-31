using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using YouYou.Business.Models.Validations;

namespace YouYou.Business.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string NickName { get; set; }

        public bool IsCompany { get; set; }

        public bool Disabled { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<ApplicationUserRole> UserRoles { get; set; }

        public ICollection<ExtraPhone> ExtraPhones { get; set; }

        public PhysicalPerson PhysicalPerson { get; set; }

        public JuridicalPerson JuridicalPerson { get; set; }

        public string TermsOfUse { get; set; }

        [NotMapped]
        public ValidationResult ValidationResult { get; protected set; }

        //EF
        public ApplicationUser() { }

        public ApplicationUser(string userName, string email)
        {
            UserName = userName;
            Email = email;
        }

        public bool IsValid()
        {
            ValidationResult = new ApplicationUserValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
