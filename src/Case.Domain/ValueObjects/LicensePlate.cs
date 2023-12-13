using Case.Domain.Exceptions;

namespace Case.Domain.ValueObjects;

public sealed record LicensePlate
{
    public string Value { get; }

    public LicensePlate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidLicensePlateException(value);
        }
        if (value.Length is < 5 or > 8)
        {
            throw new InvalidLicensePlateException(value);
        }
        
        Value = value;
    }

    public static implicit operator string(LicensePlate licensePlate)
        => licensePlate.Value;

    public static implicit operator LicensePlate(string value)
        => new(value);
}