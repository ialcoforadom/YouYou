using YouYou.Business.Models.Validations;
using YouYou.Business.Utils;

namespace YouYou.Business.Models
{
    public class PhysicalPerson : Entity
    {
        public string CPF { get; set; }

        public string Name { get; set; }

        public Guid UserId { get; set; }

        public ApplicationUser User { get; set; }
        public DateTime? Birthday { get; set; }

        public Guid GenderId { get; set; }

        public Gender Gender { get; set; }

        public PhysicalPerson() { }

        public PhysicalPerson(string cpf, string name, DateTime? birthday, Guid typeGenderId, string? description)
        {
            CPF = UsefulFunctions.RemoveNonNumeric(cpf);
            Name = name;
            Birthday = birthday;
            Gender = new Gender(typeGenderId, description);
        }

        public override bool IsValid()
        {
            ValidationResult = new PhysicalPersonValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
