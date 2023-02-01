using System.ComponentModel.DataAnnotations;

namespace YouYou.Api.Extensions
{
    public class StringLengthListAttribute : StringLengthAttribute
    {
        public StringLengthListAttribute(int maximumLength)
            : base(maximumLength) { }

        public override bool IsValid(object value)
        {
            if (!(value is List <string>))
                return false;

            foreach (var str in value as List<string>)
            {
                if (str.Length > MaximumLength || str.Length < MinimumLength)
                    return false;
            }

            return true;
        }
    }
}
