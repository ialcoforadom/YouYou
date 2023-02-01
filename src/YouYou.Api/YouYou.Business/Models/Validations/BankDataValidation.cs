using FluentValidation;

namespace YouYou.Business.Models.Validations
{
    public class BankDataValidation : AbstractValidator<BankData>
    {
        public BankDataValidation()
        {
            RuleFor(f => f.BankName)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .MaximumLength(256)
                .WithMessage("O campo {PropertyName} só pode ter no máximo {MaxLength} caracteres");

            RuleFor(f => f.Agency)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .MaximumLength(5)
                .WithMessage("O campo {PropertyName} só pode ter no máximo {MaxLength} caracteres");

            RuleFor(f => f.Account)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .MaximumLength(10)
                .WithMessage("O campo {PropertyName} só pode ter no máximo {MaxLength} caracteres");

            RuleFor(f => f.CpfOrCnpjHolder)
                .MaximumLength(14)
                .WithMessage("O campo {PropertyName} só pode ter no máximo {MaxLength} caracteres");

            RuleFor(f => f.PixKey)
                .MaximumLength(32)
                .WithMessage("O campo {PropertyName} só pode ter no máximo {MaxLength} caracteres");
        }
    }
}
