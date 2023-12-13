namespace Case.Domain.Exceptions;

public sealed class InvalidEmployeeNameException : CustomException
{
    public InvalidEmployeeNameException() : base("Employee name is invalid.")
    {
    }
}