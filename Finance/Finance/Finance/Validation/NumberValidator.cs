using System.Text.RegularExpressions;

namespace Finance.Validation
{
    public class NumberValidator : IValidationRule
    {
        const string pattern = @"^[0-9]+(\.[0-9]+)?$";

        public bool Validate(string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length > 9) return false;
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(value);
        }
    }
}
