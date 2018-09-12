namespace Finance.Validation
{
    public interface IValidationRule
    {
        bool Validate(string value);
    }
}
