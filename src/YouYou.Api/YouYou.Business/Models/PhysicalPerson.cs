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

        public PhysicalPerson() { }

        public PhysicalPerson(string cpf, string name)
        {
            CPF = UsefulFunctions.RemoveNonNumeric(cpf);
            Name = name;
        }

        public override bool IsValid()
        {
            ValidationResult = new PhysicalPersonValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
