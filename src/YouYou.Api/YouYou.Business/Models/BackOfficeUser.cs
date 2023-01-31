using YouYou.Business.Models.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace YouYou.Business.Models
{
    public class BackOfficeUser : Entity
    {
        public Guid UserId { get; set; }

        public ApplicationUser User { get; set; }

        public bool IsDeleted { get; set; }

        public BackOfficeUser() { }

        public BackOfficeUser(string name, string cpf, string email)
        {
            User = new ApplicationUser(email, email);
            User.PhysicalPerson = new PhysicalPerson(cpf, name);
        }

        public override bool IsValid()
        {
            ValidationResult = new PhysicalPersonValidation().Validate(this.User.PhysicalPerson);
            ValidationResult.Errors.AddRange(new ApplicationUserValidation().Validate(this.User).Errors);

            return ValidationResult.IsValid;
        }
    }
}
