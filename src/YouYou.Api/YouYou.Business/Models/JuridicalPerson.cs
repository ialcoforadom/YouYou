using System;
using System.Collections.Generic;
using System.Text;

namespace YouYou.Business.Models
{
    public class JuridicalPerson : Entity
    {
        public string CNPJ { get; set; }

        public string CompanyName { get; set; }

        public Guid UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
