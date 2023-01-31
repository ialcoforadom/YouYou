using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation.Results;

namespace YouYou.Business.Models
{
    public abstract class Entity
    {

        public Guid Id { get; set; }

        [NotMapped]
        public ValidationResult ValidationResult { get; protected set; }

        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public virtual bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
