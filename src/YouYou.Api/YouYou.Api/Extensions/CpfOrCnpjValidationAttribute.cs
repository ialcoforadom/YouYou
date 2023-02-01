using System.ComponentModel.DataAnnotations;
using YouYou.Business.Utils;

namespace YouYou.Api.Extensions
{
    /// <summary>
    /// Validação customizada para CPF ou CNPJ
    /// </summary>
    public class CpfOrCnpjValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// Construtor
        /// </summary>
        public CpfOrCnpjValidationAttribute() { }

        /// <summary>
        /// Validação server
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return true;

            string CpforCNPJ = UsefulFunctions.RemoveNonNumeric(value.ToString());

            if(CpforCNPJ.Length > 11)
            {
                return UsefulFunctions.ValidateCNPJ(CpforCNPJ);
            }

            return UsefulFunctions.ValidateCpf(CpforCNPJ);
        }
    }
}
